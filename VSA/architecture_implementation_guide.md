# Kurumsal Proje Yönetimi: İleri Seviye Mimari Rehberi

Bu doküman, projeye **MediatR**, **Audit Interceptor** ve **Event Sourcing (Domain Events)** kavramlarının nasıl entegre edileceğini adım adım açıklar.

---

## 1. MediatR ve Vertical Slice (VSA) Entegrasyonu

MediatR, dikey dilim mimarisinin temel taşıdır. İş mantığını controller'lardan ayırarak bağımsız dilimler oluşturmamızı sağlar.

### Uygulama Adımları:
1. **Paket Kurulumu**: 
   ```bash
   dotnet add package MediatR
   ```
2. **Yapılandırma (`Program.cs`)**:
   ```csharp
   builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
   ```
3. **Klasör Yapısı**: `Features/Projects/CreateProject/` adında bir klasör oluşturulur.
4. **Kod Örneği**:
   - `CreateProjectCommand.cs`: İstek verileri.
   - `CreateProjectHandler.cs`: İş mantığının (business logic) koşturulduğu yer.
   - `ProjectsController.cs`: Sadece `_mediator.Send(command)` çağrısı yapar.

---

## 2. Audit Interceptor (Otomatik İzleme)

Veritabanına bir kayıt eklendiğinde veya güncellendiğinde `CreatedAt`, `LastModifiedBy` gibi alanların otomatik dolmasını sağlar.

### Uygulama Adımları:
1. **Arayüz Tanımı**: `IAuditableEntity` adında bir interface oluşturulur.
2. **Interceptor Yazımı**: `SaveChangesInterceptor` sınıfından türeyen `AuditInterceptor` yazılır.
3. **Mantık**: `SavingChangesAsync` metodunda, `ChangeTracker` üzerinden `Added` veya `Modified` olan nesneler yakalanır ve tarihleri set edilir.
4. **Kayıt (`Program.cs`)**:
   ```csharp
   builder.Services.AddDbContext<AppDbContext>((sp, options) => {
       options.UseSqlServer(connectionString)
              .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
   });
   ```

---

## 3. Event Sourcing (Domain Events ile Başlangıç)

Sistemde olan biten önemli olayları (örn: Proje Oluşturuldu, Görev Atandı) kaydetmek ve bu olaylara tepki vermek için kullanılır.

### Uygulama Adımları:
1. **Base Entity Güncelleme**: `BaseEntity` sınıfına bir `List<IDomainEvent>` eklenir.
2. **Olay Ekleme**: `Project` oluşturulduğunda constructor içinde `AddDomainEvent(new ProjectCreatedEvent(this))` çağrılır.
3. **Dispatch (Dağıtım)**: `AppDbContext` içinde `SaveChangesAsync` bittikten sonra, kaydedilen entity'lerdeki event'ler MediatR üzerinden publish edilir.
4. **Handler**: `ProjectCreatedEventHandler` bu olayı yakalayıp e-posta göndermek veya başka bir servise haber vermek gibi işlemleri yapar.

---

### Özet Avantajlar:
- **MediatR**: Kodun test edilebilirliğini ve sürdürülebilirliğini artırır.
- **Audit**: Hataya yer bırakmadan güvenli bir denetim izi (audit trail) oluşturur.
- **Domain Events**: Sistemler arası gevşek bağlı (loosely coupled) bir iletişim sağlar ve tam teşekküllü **Event Sourcing** için temel oluşturur.
