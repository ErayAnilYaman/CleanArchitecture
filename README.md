# 🧱 Clean Architecture Setup – 2025

Bu repo, **2025 yılı projeleri** için başlangıç noktası olarak kullanılabilecek, modern ve ölçeklenebilir bir **Clean Architecture** iskeleti sunar. Hedef; projelerde **tekrarlanabilirlik**, **sürdürülebilirlik**, **test edilebilirlik** ve **net sorumluluk ayrımı** sağlamaktır.

---

## 🧩 Mimari Yaklaşım

**Architectural Pattern**
- Clean Architecture

**Design Patterns**
- Result Pattern
- Repository Pattern
- CQRS Pattern
- Unit of Work Pattern

---

## ⚙️ Kütüphaneler

- **MediatR** – Komut/Sorgu (CQRS) akışları ve handler bazlı orkestrasyon
- **TS.Result** – Başarı/Hata odaklı fonksiyonel sonuç modeli
- **Mapster** – DTO ⇄ Entity hızlı ve hafif nesne eşleme
- **FluentValidation** – Giriş/doğrulama kuralları
- **TS.EntityFrameworkCore.GenericRepository** – Generic Repository altyapısı
- **EntityFrameworkCore** – ORM ve veritabanı işlemleri
- **OData** – API seviyesinde filtreleme/sıralama/projeksiyon
- **Scrutor** – Assembly taraması ile otomatik DI kayıtları

> Not: Paket sürümleri `Directory.Packages.props` veya proje dosyasında merkezi olarak yönetilebilir.

---

## 📂 Katmanlar

```
src/
├─ Presentation/           # API veya UI katmanı (WebAPI)
├─ Application/            # Use case'ler, DTO/Contracts, CQRS, Validation
├─ Domain/                 # Entity'ler, Value Object'ler, Domain Events, Abstraction'lar
├─ Infrastructure/         # EF Core, Repository, UoW, 3rd-party adaptörler
└─ Shared/                 # Ortak util/helper (opsiyonel)
tests/
├─ UnitTests/
└─ IntegrationTests/
```

- **Domain**: İş kuralları ve çekirdek model. Çerçeve-bağımsızdır.
- **Application**: Use case mantığı; `MediatR` komut/sorgu handler’ları, `FluentValidation` ve `TS.Result` burada.
- **Infrastructure**: Veri erişimi (EF Core), repository & unit of work, dış servis adaptörleri.
- **Presentation**: HTTP endpoint’ler, OData konfigürasyonu, DI kabulleri.

---

## 🧠 Konvansiyonlar

- **CQRS**: Yazma (Command) ve okuma (Query) senaryoları ayrıştırılır.
- **Result Pattern**: Tüm use case sonuçları `Result<T>` olarak döner. Hata mesajları ve error codes standarttır.
- **Validation**: `FluentValidation` ile request bazlı kurallar; handler’dan önce çalışır.
- **Mapping**: `Mapster` profil/registration’ları Application katmanında tanımlıdır.
- **DI/IoC**: `Scrutor` ile otomatik tarama—`IAssemblyMarker` tipi üzerinden ilgili assembly’ler kaydedilir.
- **Persistence**: Transaction yönetimi `UnitOfWork` ile yapılır.

---

## 🚀 Hızlı Başlangıç

1. Depoyu klonla.
2. `Presentation` katmanında `appsettings.json` değerlerini düzenle (DB connection vs.).
3. EF Core migration ve veritabanı güncelle:
   ```bash
   dotnet ef migrations add Initial --project src/Infrastructure --startup-project src/Presentation
   dotnet ef database update --project src/Infrastructure --startup-project src/Presentation
   ```
4. Uygulamayı çalıştır:
   ```bash
   dotnet run --project src/Presentation
   ```

---

## 🧭 Örnek Akış (CQRS + Result + Validation)

**CreateProductCommand.cs**
```csharp
public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<Result<Guid>>;
```

**CreateProductCommandValidator.cs**
```csharp
public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

**CreateProductCommandHandler.cs**
```csharp
public sealed class CreateProductCommandHandler(
    IProductRepository repo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var exists = await repo.ExistsByNameAsync(request.Name, ct);
        if (exists) return Result.Fail<Guid>("ProductAlreadyExists");

        var entity = new Product(request.Name, request.Price);
        await repo.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        return Result.Ok(entity.Id);
    }
}
```

**ProductsController.cs (Presentation)** – OData + MediatR
```csharp
[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, null)
                                : Problem(detail: string.Join(";", result.Errors), statusCode: 400);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
}
```

---

## 🧰 DI Kayıtları (Scrutor)

```csharp
services.Scan(scan => scan
    .FromAssembliesOf(typeof(IApplicationMarker)) // Application
        .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces().WithScopedLifetime()
        .AddClasses(c => c.InNamespaces("Application.UseCases"))
            .AsImplementedInterfaces().WithScopedLifetime()
    .FromAssembliesOf(typeof(IInfrastructureMarker)) // Infrastructure
        .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces().WithScopedLifetime()
        .AddClasses(c => c.Where(t => t.Name.EndsWith("UnitOfWork")))
            .AsImplementedInterfaces().WithScopedLifetime()
);
```

---

## 🔍 OData Hızlı Kurulum

```csharp
services.AddControllers().AddOData(opt =>
    opt.Filter().Select().OrderBy().Count().Expand().SetMaxTop(100));
```

- Endpoint’lerde `IQueryable` dönüşleri için **sadece okuma** senaryolarında OData kullanın.
- Yazma (POST/PUT/PATCH/DELETE) için klasik REST akışı tercih edilir.

---

## ✅ Sağlamlık & Test

- **UnitTests**: Handler ve validator’lar için izole testler
- **IntegrationTests**: EF Core + gerçek DB (veya test container) ile uçtan uca
- **Validation Pipeline**: Handler öncesi otomatik doğrulama için `IPipelineBehavior<T>`

---

## 🗺️ Yol Haritası

- [ ] Örnek Feature: `Products` – CRUD + OData Query
- [ ] Pipeline Behaviors: Validation, Logging, Performance
- [ ] Global Exception Handling & ProblemDetails
- [ ] Serilog + OpenTelemetry entegrasyonu
- [ ] Docker Compose (DB + API) geliştirme ortamı
- [ ] CI/CD (GitHub Actions) – build/test/lint

---

## 📜 Lisans

Ticari/kurumsal kullanım için kurum içi lisans politikalarına tabidir.
