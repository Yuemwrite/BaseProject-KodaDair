## .NET CORE İle Backend Alt Yapı ve KodaDair Projesi

![resim](https://github.com/Yuemwrite/BaseProject-KodaDair/assets/32547627/2d00b58d-4eb4-4a03-bacc-de5f0068704a)

<h3><strong>İçindekiler 📄<strong></h3>

* [Giriş ve Tanışma](#giris-ve-tanisma)
  
* [Proje Mimarisinde Kullanılan Yapılar](#proje-mimari)
  * [Katmanlı Mimari](#katmanli-mimari)
  * [Onion Architecture](#onion-architecture)
  * [Repository Pattern](#repository-pattern)
  * [Dependency Injection](#dependency-injection)
  * [CQRS](#cqrs)
  * [Code First](#code-first)

* [Proje Alt Yapısında Yer Verilen Yapılar](#alt-yapi)
   * [Cache](#cache)
     * [Redis](#redis-cache)
     * [Memory Cache](#memory-cache)
   * [Ocelot Gateway](#ocelot-gateway)
   * [JWT Token](#jwt-token)
   * [Refresh Token](#refresh-token)
   * [Log](#log)
     * [Seri Log](#seri-log)
     * [SEQ](#seq)
   * [Quartz](#quartz)
   * [Rabbit MQ](#rabbitmq)
   * [Exception Middleware](#exception-middleware)
   * [Auto Mapper](#AutoMapper)
   * [Seed Yapısı](#seed-yapi)
   * [Saylalama Yapısı(Pagination)](#pagination)
   * [Environment Yapsısı](#environment)

* [KodaDair](#koda-dair)
   * [KodaDair Konusu Nedir?](#kodadair-konu)
   * [KodaDair Yer Alan Modüller](#kodadair-modul)
   * [KodaDair Hedeflenen Modüller](#kodadair-hedef)
   * [KodaDair Kullanılan API'ler](#kodadair-api)
 
* [Projeye İlişkin Diğer Bilgiler](#diger)
 
* [Kapanış](#kapanis)
      
* [Kaynakçalar](#kaynakca)

# <h2 id="giris-ve-tanisma"><strong>Giriş ve Tanışma<strong></h2>

Merhabalar, ben Yunus Emre 👋

Karadeniz Teknik Üniversitesi Yazılım Mühendisliği bölümünden mezun oldum. 2 yıla yakındır yazılım mühendisi olarak özel bir firmada .net core / c# teknolojileri (backend developer) alanında  çalışmalar yapıyorum. 2 yıl boyunca içinde olduğum projelerden elde ettiğim deneyimler, araştırdığım kaynaklar, makaleler ve izlediğim eğitimlerden öğrendiğim bilgileri bir araya toplamak amacıyla .net core / web api tarafında bir alt yapı hazırlamaya ve hazırladığım alt yapıya da örnek bir proje senaryosu eklemeye karar verdim. Bu karar neticesinde içerisinde sektörde kullanılan ve talep edilen birçok farklı .net kütüphanesini kullanarak bir proje ortaya çıkarmaya çalıştım. En temel amacım insanların farklı konuları farklı kaynaklarda aramak yerine her bir konuyu tek bir çatı altında bulmalarına olanak sağlamak ve gün geçtikçe içeriği zenginleştirerek insanlara daha faydalı olmaktır. Ayrıca sürekli gelişen ve büyüyen .NET / .NET Core alanında birçokları tarafından yazılan makaleler, çekilen video eğitimler gibi kaynakların bol olduğu zincir ben de hazırlamış olduğum bu kaynak ile bir halka eklemek ve zincire dahil olmak istedim. Oluşturduğum kaynağı beğenmeniz dileğiyle.. :blush:

*Sizler de kaynağa destek ve [![](https://img.shields.io/static/v1?label=Sponsor&message=%E2%9D%A4&logo=GitHub&color=%23fe8e86)](https://github.com/sponsors/yuemwrite) olabilir, kaynağın daha çok kişiye erişmesini sağlayabilirsiniz.* :dizzy:

# <h1 id="proje-mimari"><strong>Proje Mimarisinde Kullanılan Yapılar<strong></h2>

### <h2 id="katmanli-mimari"><strong>Katmanlı Mimari<strong></h2>

Katmanlı mimari, bir yazılım uygulamasının farklı işlevleri ve sorumlulukları olan bileşenlerini farklı katmanlara ayırmayı ve bu katmanlar arasında belirli bir düzende iletişim kurmayı sağlayan bir tasarım desenidir. Genel olarak, katmanlı mimari üç ana katmanı içerir:

Sunum Katmanı (Presentation Layer): Kullanıcı arayüzünü (UI) temsil eder. Bu katman, kullanıcının uygulama ile etkileşimini sağlayan bileşenlerden oluşur.

İş Katmanı (Business Layer): Uygulamanın iş mantığına odaklanır. Bu katmanda, uygulamanın temel işlevleri gerçekleştirilir ve iş kuralları uygulanır.

**Veri Katmanı (Data Layer)**: Veritabanı ve diğer veri kaynaklarına erişim sağlar. Bu katman, verilerin depolanması, alınması ve güncellenmesi gibi işlemleri gerçekleştirir.

Katmanlı mimari, bir uygulamanın daha modüler, esnek ve yönetilebilir olmasını sağlar. Ayrıca, katmanlar arasındaki bağımlılıkları azaltarak kodun daha kolay test edilmesine ve yeniden kullanılmasına olanak tanır. Bazı katmanlı mimarinin avantajları şunlardır:

**Yeniden Kullanılabilir Kod**: Katmanlı mimari, kodun yeniden kullanımını kolaylaştırır. Çünkü her katman, kendine özgü işlevleri yerine getiren bağımsız bir modül olarak tasarlanır.

**Kolay Bakım**: Kodun daha yönetilebilir olması sayesinde, katmanlı mimari ile yazılan uygulamaların bakımı daha kolaydır. Değişiklikler yapmak veya yeni özellikler eklemek daha kolaydır.

**Daha Az Bağımlılık**: Katmanlı mimari, katmanlar arasındaki bağımlılıkları azaltarak uygulamanın daha esnek olmasını sağlar. Bir katmandaki değişiklikler, diğer katmanlara minimal bir etki yapar.

**Test Edilebilir Kod**: Her katman, kendine özgü işlevleri yerine getiren bir modül olduğu için, bu katmanların her biri kolayca test edilebilir.

**Gelişmiş Güvenlik**: Katmanlı mimari, güvenliği artıran birçok özellik sunar. Veri katmanı, verilerin doğru bir şekilde depolanmasını ve korunmasını sağlar. İş katmanı, iş kurallarının uygulanmasını ve doğru çalışmasını sağlar. Sunum katmanı, doğru erişim yetkilerinin sağlanmasını sağlar.

Bu nedenlerden dolayı, katmanlı mimari yaygın olarak kullanılan bir yazılım tasarım desenidir.

### <h2 id="onion-architecture">Onion Architecture</h2>
 
![resim](https://github.com/Yuemwrite/BaseProject-KodaDair/assets/32547627/51e70074-89ff-4fa1-8bbc-592029d0b5c8)

 
Onion (Soğan) mimarisi, yazılım uygulamalarını geliştirmek için kullanılan bir mimari modeldir. Bu mimaride uygulama, farklı katmanlardan oluşan bir yapıya sahiptir. Her katmanın belli bir sorumluluğu vardır ve katmanlar arasındaki bağımlılık, dıştan içe doğru bir yapı şeklinde düzenlenir.

Onion mimarisindeki katmanlar şunlardır:

Domain Katmanı (Çekirdek Katmanı): Bu katman, uygulamanın iş mantığını ve veri modellerini içerir. İş mantığı, uygulamanın temel amacını ve işlevlerini oluştururken, veri modelleri ise uygulamanın verilerini nasıl saklayacağını tanımlar. Domain katmanı, diğer katmanlarla bağımsız bir şekilde geliştirilir.

Application Katmanı (Uygulama Katmanı): Bu katman, uygulamanın iş mantığını kullanarak, uygulamanın farklı taleplerine cevap verir. Bu katmanda, kullanıcı arayüzü (UI), API, arka planda çalışan servisler vb. yer alabilir.

Infrastructure Katmanı (Altyapı Katmanı): Bu katman, uygulamanın alt yapı elemanlarını içerir. Bu elemanlar, veri tabanı, dosya sistemi, ağ bağlantıları vb. gibi uygulamanın dışarıya bağlandığı bileşenlerdir. Bu katman, diğer katmanlara hizmet etmek için kullanılır.

UI Katmanı (Kullanıcı Arayüzü Katmanı): Bu katman, kullanıcıların uygulamayı kullanabilmesi için gerekli olan arayüzü içerir. UI katmanı, kullanıcıların uygulamayla etkileşim kurduğu yerdir.

Onion mimarisi, birçok avantajı sağlar:

* Bağımsızlığı sağlar: Her katmanın, diğer katmanlardan bağımsız bir şekilde geliştirilmesi, kodu daha kolay bakım yapılabilir hale getirir.
* Değişiklikleri kolaylaştırır: Bir katmanda yapılan değişiklikler, diğer katmanlara minimal etki eder ve uygulamanın geliştirilmesi daha esnek hale gelir.
* Test edilebilirliği artırır: Her katmanın belirli bir işlevi olduğu için, katmanlar arasındaki sınır daha net bir şekilde belirlenir ve test edilebilirlik artar.
* Esnekliği sağlar: İş mantığını ve veri modellerini içeren Domain katmanı, uygulamanın çekirdeğini oluşturur ve uygulama için belirlenen gereksinimlerde kolayca değişiklik yapılabilmesini sağlar.

### <h2 id="#repository-pattern"><strong>Repository Pattern<strong></h2>
 
Repository Pattern, yazılım geliştirme sürecinde kullanılan bir tasarım desenidir. Temel amacı, veri erişim katmanının (data access layer) işlevselliğini soyutlama ve modülerleştirme sağlamaktır. Repository Pattern, veritabanıyla ilgili işlemleri gerçekleştiren sınıfları ve bunlarla etkileşim sağlayan bir arabirim (interface) aracılığıyla kullanır. Bu şekilde, veri erişim katmanı kodu bağımsız hale gelir ve daha test edilebilir ve bakımı kolay bir yapı oluşturulur.

Repository Pattern'in bazı ana özellikleri şunlardır:

Modülerlik: Repository Pattern, veri erişim katmanını soyutlama ve bağımsızlaştırma yoluyla modüler bir yapı sağlar. Bu sayede, veri erişim kodunun diğer iş mantığı kodlarından ayrılması ve değişikliklerin daha kolay uygulanması sağlanır.

Tek Sorumluluk İlkesi: Repository Pattern, her bir veri tabanı tablosu veya varlık için ayrı bir repository sınıfı oluşturmayı teşvik eder. Bu şekilde, her repository sınıfı yalnızca o varlığın veri erişim işlemleriyle ilgilenir ve tek sorumluluk ilkesine uygun bir şekilde kodlanmış olur.

Veritabanı Bağımsızlığı: Repository Pattern, veritabanıyla etkileşimi soyutlama yoluyla gerçekleştirir. Bu sayede, veritabanı teknolojisi değiştiğinde veya veri erişim yöntemi değiştirildiğinde, kodun diğer kısımlarında minimal değişiklik yapılması yeterli olur.

Test Edilebilirlik: Repository Pattern, veri erişim katmanını bağımsız hale getirerek, daha kolay test edilebilir bir yapı sunar. Repository sınıfları, veri erişimine ilişkin işlemleri yürütürken mock veya sahte (fake) verilerle test edilebilir.

Repository Pattern'in kullanılması, yazılım projelerinde çeşitli avantajlar sağlar. Bunlar arasında kodun daha okunabilir ve bakımı kolay olması, veri erişim katmanının değişikliklere karşı dirençli hale gelmesi, test edilebilirliğin artması ve veritabanı bağımlılığının azalması gibi faktörler bulunmaktadır.
 
 <code>IEntityRepository<T></code>
```csharp

 public interface IEntityRepository<T> :  IEntityGeneralRepository where T : class, IEntity
{
    T Add(T entity);
    T Update(T entity);
    T Delete(T entity);
    IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression = null, bool asNoTracking = true, CancellationToken cancellationToken = default);
    T Get(Expression<Func<T, bool>> expression);
    Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    IQueryable<T> Query();
}

```
 
### <h2 id="#dependency-injection"><strong>Dependency Injection<strong></h2>
 
Dependency Injection (Bağımlılık Enjeksiyonu), bir nesnenin, bağımlı olduğu diğer nesneleri dışarıdan alması ve bu bağımlılıkların dışarıdan yönetilmesi anlamına gelir. Repository Pattern ile birlikte Dependency Injection kullanmak, bağımlılıkların yönetimini kolaylaştırır ve kodun daha esnek hale gelmesini sağlar.
  
  <code>Repository/Dependency Injection</code>
```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IEntityRepository<User> _userRepository;
        
        public CreateUserCommandHandler(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

```
  
Yukarıdaki örnekte, CreateUserCommand sınıfı, IRepository<User> arayüzüne bağımlıdır. Dependency Injection kullanarak, bu bağımlılığı enjekte ediyoruz. Böylece, CreateUserCommand sınıfının IRepository<User> nesnesine erişimi gerçekleşiyor ve kodun daha esnek ve test edilebilir olmasını sağlıyoruz.
 
 ### <h2 id="#cqrs"><strong>CQRS<strong></h2> 
  
![resim](https://yunusemrehaslak.com/wp-content/uploads/2022/01/cqrspattern-1140x526.jpg)
  
CQRS (Command Query Responsibility Segregation), yazılım geliştirme mimarilerinde bir tasarım deseni veya prensibidir. CQRS, komut (command) ve sorgu (query) sorumluluklarını ayırarak, veri okuma ve veri yazma işlemlerini farklı şekillerde ele almayı önerir.

Geleneksel bir mimaride, veritabanı işlemleri hem sorgu hem de komut operasyonlarını aynı şekilde gerçekleştirir. Ancak, bir uygulamanın gereksinimleri genellikle sorgu ve komut işlemleri için farklılık gösterebilir. CQRS, bu gereksinimleri karşılamak için veri okuma ve veri yazma süreçlerini ayrı ayrı ele alır.

CQRS'in ana fikri, veritabanı işlemlerinin iki ayrı kısmı olan bir komut tarafı (command side) ve bir sorgu tarafı (query side) oluşturmaktır.

Komut Tarafı:

Veri ekleme, güncelleme veya silme gibi veri değişikliklerini gerçekleştirir.
İşlemler genellikle değişiklik yapacak komutlar (command) olarak adlandırılır.
Sıklıkla Event Sourcing ve/veya Domain-Driven Design (DDD) prensipleriyle birlikte kullanılır.
  
Sorgu Tarafı:

Veri okuma işlemlerini gerçekleştirir.
İşlemler genellikle veriyi sorgulayan sorgular (query) olarak adlandırılır.
Veri okuma işlemlerinin optimize edilmesi, raporlamalar ve sorgu tarafında özel ihtiyaçların karşılanması sağlanır.

### <h2 id="#code-first"><strong>Code First<strong></h2> 

Code First, bir yazılım geliştirme yaklaşımı ve veritabanı tasarım yaklaşımını ifade eder. Bu yaklaşım, özellikle Entity Framework gibi ORM (Object-Relational Mapping) araçlarıyla ilişkilendirilir ve veritabanı şemasını, sınıfları ve nesneleri oluşturarak programlama kodlarıyla tanımlamaya dayanır.

Code First yaklaşımı şu adımlarla çalışır:

 - Sınıf Tanımlamaları: Öncelikle, uygulamanızda kullanacağınız nesneleri ve sınıfları tanımlarsınız. Bu sınıflar, genellikle uygulamanızın iş mantığını temsil eder. Örneğin, bir e-ticaret uygulaması için Ürün, Sipariş ve Müşteri gibi sınıflar tanımlayabilirsiniz.

 - İlişki Tanımları: Sınıflar arasındaki ilişkileri ve bağlantıları belirtirsiniz. Örneğin, bir Sipariş sınıfının bir Müşteri sınıfına ait olabileceğini belirtebilirsiniz.

 - Veritabanı Bağlamı (DbContext) Oluşturma: Veritabanı bağlamı, uygulamanızdaki sınıfları ve ilişkileri temel alan bir sınıftır. Bu sınıfı oluşturarak, sınıflarınızı ve ilişkilerinizi veritabanında nasıl temsil edeceğinizi belirtirsiniz.

 - Veritabanını Oluşturma veya Güncelleme: Bu aşamada, belirttiğiniz sınıf ve ilişki tanımlamalarını kullanarak gerçek bir veritabanı şeması oluşturulur veya güncellenir. Bu işlem otomatik olarak yapılır ve genellikle Entity Framework tarafından yönetilir.

<h2 name="alt-yapi"><strong>Proje Alt Yapısında Yer Verilen Yapılar<strong></h2>

<h3 name="cache"><strong>Cache<strong></h3>

Cache (ön bellek), bir sistemde sık kullanılan verileri veya işlemleri geçici olarak saklamak için kullanılan hızlı erişimli bir bellek alanıdır. Cache, verilere daha hızlı erişim sağlamak ve sistemin performansını artırmak için kullanılır.

Bir sistemde, verilere erişim genellikle daha yavaş olan kaynaklardan (örneğin, disk veya ağ) gerçekleştirilir. Bu durumda, aynı verilere tekrar tekrar erişmek gerektiğinde, her seferinde yavaş kaynaklardan veri almak sistem performansını olumsuz etkileyebilir.

Cache, bu sorunu çözmek için araya girer. Sık kullanılan verileri veya işlemleri hızlı bir bellek alanında saklayarak, sonraki erişimlerde veriye daha hızlı erişim sağlar. Öncelikle yavaş kaynaktan (örneğin, veritabanından veya ağdan) veri alır ve bu verileri cache'e kaydeder. Ardından, aynı veriye bir sonraki erişimde cache'ten hızlı bir şekilde erişilebilir.

  
# <h3 name="redis-cache"><strong>Redis<strong></h3>
  
![resim](https://github.com/Yuemwrite/BaseProject-KodaDair/assets/32547627/b7b17c1f-22de-4999-b66b-9191542cab69)

  
Redis, açık kaynaklı bir veri yapısı sunucusu ve cache (ön bellek) çözümüdür. Redis, RAM tabanlı çalışır ve verileri hızlı bir şekilde depolamak ve erişmek için optimize edilmiştir. Redis, hafızada bulunan verilere erişim hızını artırırken, disk tabanlı veritabanlarının gerekli olduğu durumlarda yükü azaltmak için kullanılabilir.
  
  <code>IRedisService</code>
```csharp
public interface IRedisService
{
    T? Get<T>(string key);
    void Add(string key, object data, TimeSpan timeSpan);
    void Remove(string key);
    bool Any(string key, RedisKeyType redisKeyType);
    
}

```
  
  
<code>RedisService</code>
```csharp
public class RedisService : IRedisService
{
    readonly RedisClient _redisClient = new RedisClient();

    public RedisService(IConfiguration configuration)
    {
        RedisConnect(configuration);
    }

    public T Get<T>(string key)
    {
        return _redisClient.Get<T>(key);
    }

    public void Add(string key, object data, TimeSpan timeSpan)
    {
        _redisClient.Set(key, data, timeSpan);
    }

    public void Remove(string key)
    {
        _redisClient.Remove(key);
    }

    public bool Any(string key, RedisKeyType redisKeyType)
    {
        throw new NotImplementedException();
    }


    public bool Any(string key)
    {
        return true;
    }

    private RedisClient RedisConnect(IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>();
        
        var redisClient = new RedisClient(redisConfiguration!.Host, redisConfiguration!.Port);

        return redisClient;
    }
}
```
  
Örnek Kullanım
  
<code>Redis Örnek Kullanım</code>
```csharp
public async Task<Result> Handle(GetNoCacheUsersQuery request, CancellationToken cancellationToken)
        {
            List<User> result = _redisService.Get<List<User>>("users");
            if (result is not null)
            {
                _redisService.Remove("users");
                return await Result<List<User>>
                    .SuccessAsync(result);
            }

            result = _userRepository.Query().ToList();
            _redisService.Add("users", result, TimeSpan.FromMinutes(10));


            return await Result<List<User>>
                .SuccessAsync(result);
        }

```
  
Bu kod örneği, Redis cache kullanarak bir "users" listesini önbellekte saklamak ve erişimde hız kazanmak amacıyla kullanılmıştır.

İlk olarak, _redisService.Get<List<User>>("users") ifadesi ile "users" anahtarını kullanarak Redis cache'ten bir değer alınmaya çalışılır. Eğer bu değer null değilse (yani Redis cache'te "users" anahtarı bulunuyorsa):

a. _redisService.Remove("users") ifadesi ile Redis cache'te bulunan "users" anahtarını kaldırır. Böylece, güncel verilerin alınması için cache temizlenir.

b. Ardından, Result<List<User>>.SuccessAsync(result) ifadesi ile Redis cache'ten alınan değeri başarılı bir şekilde döndürür.

Eğer Redis cache'te "users" anahtarı bulunmuyorsa (yani cache'te null değer döndürüldüyse):

a. _userRepository.Query().ToList() ifadesi ile kullanıcıları veritabanından alır.

b. _redisService.Add("users", result, TimeSpan.FromMinutes(10)) ifadesi ile "users" anahtarıyla Redis cache'e kullanıcıları ekler. Bu cache, 10 dakika boyunca geçerli olacak şekilde ayarlanır.

c. Son olarak, Result<List<User>>.SuccessAsync(result) ifadesi ile kullanıcıları başarılı bir şekilde döndürür.

Bu kod parçası, önce Redis cache'ten "users" anahtarını kontrol eder ve eğer cache'te varsa, verileri cache'ten alarak hızlı bir şekilde döndürür. Eğer cache'te "users" anahtarı bulunmuyorsa, veritabanından verileri alır, Redis cache'e ekler ve sonuçları döndürür. Bu şekilde, sık kullanılan veriler cache'te saklanır ve daha hızlı erişim sağlanır.
  
## <h3 id="memory-cache"><strong>Memory Cache<strong></h2>
  
Memory Cache, .NET Framework ve .NET Core gibi platformlarda sunulan bir bellek tabanlı önbellek çözümüdür. Memory Cache, uygulama içinde sık kullanılan verileri geçici olarak bellekte saklar ve hızlı bir erişim sağlar.
  
Memory Cache, önbellekleme, performans optimizasyonu ve sık kullanılan verilere hızlı erişim sağlama gibi senaryolarda yaygın olarak kullanılır. Uygulamalar, veritabanı sorguları, web hizmeti çağrıları veya hesaplama işlemleri gibi yavaş kaynaklara yapılan erişimleri azaltmak ve daha iyi bir kullanıcı deneyimi sunmak için Memory Cache'i kullanabilir.


<code>Memory Cache</code>
```csharp
public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            
            
            List<User>? users = new List<User>();

            users = _memoryCache.Get<List<User>>("users");
            if (users is null)
            {
               users =  await _userRepository.Query().Take(1000).ToListAsync();

               _memoryCache.Set("users", users, TimeSpan.FromSeconds(10));
            }
            
            
            return await Result<List<User>>
                .SuccessAsync(users);
        }

```
  
Bu kod örneği, Memory Cache kullanarak "users" adlı bir liste nesnesini geçici olarak bellekte saklamak ve erişimde hız kazanmak amacıyla kullanılmıştır.

Öncelikle, List<User>? users = new List<User>(); ifadesiyle bir users listesi nesnesi oluşturulur ve başlangıçta null olarak atanır.

_memoryCache.Get<List<User>>("users") ifadesiyle "users" anahtarını kullanarak Memory Cache'ten bir değer alınmaya çalışılır. Eğer bu değer null ise (yani Memory Cache'te "users" anahtarı bulunmuyorsa):

a. await _userRepository.Query().Take(1000).ToListAsync() ifadesiyle veritabanından en fazla 1000 kullanıcı kaydını alır. (Burada varsayılan olarak Entity Framework ile bir veritabanı sorgusu kullanıldığı varsayılmıştır.)

b. _memoryCache.Set("users", users, TimeSpan.FromSeconds(10)) ifadesiyle "users" anahtarıyla Memory Cache'e kullanıcı listesini ekler. Bu cache, 10 saniye boyunca geçerli olacak şekilde ayarlanır.

c. Son olarak, Result<List<User>>.SuccessAsync(users) ifadesiyle kullanıcı listesini başarılı bir şekilde döndürür.

Eğer Memory Cache'te "users" anahtarı bulunuyorsa, direkt olarak Result<List<User>>.SuccessAsync(users) ifadesiyle bellekteki kullanıcı listesini döndürür.

Bu kod parçası, önce Memory Cache'ten "users" anahtarını kontrol eder ve eğer cache'te varsa, verileri cache'ten alarak hızlı bir şekilde döndürür. Eğer cache'te "users" anahtarı bulunmuyorsa, veritabanından verileri alır, Memory Cache'e ekler ve sonuçları döndürür. Bu şekilde, sık kullanılan veriler cache'te saklanır ve daha hızlı erişim sağlanır. Cache süresi burada 10 saniye olarak belirlenmiştir, yani bu süre boyunca aynı veriye hızlı bir şekilde erişilebilir.

# <h3 name="ocelot-gateway"><strong>Ocelot Gateway<strong></h3>

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/71604b23-4673-4728-964d-814bf7e79d2b)


Ocelot, bir API Gateway çerçevesidir. API Gateway, bir sistemdeki farklı mikro hizmetlerin veya backend servislerin önünde bulunan ve bu servislerin yönetimini, güvenliğini, performansını ve ölçeklenebilirliğini sağlamak için kullanılan bir ara katmandır. Ocelot, bu rolü yerine getiren popüler bir API Gateway çözümüdür.

Ocelot, özellikle mikro hizmet mimarilerinde, istemcilerin tek bir noktadan tüm backend servislerle etkileşimde bulunmasını sağlayan bir reverse proxy'dir. İstemciler, API Gateway üzerinden istek gönderir, Ocelot ise bu isteği hedeflenen backend servise yönlendirir. Bu, istemcilerin tüm farklı hizmetlerin adreslerini bilmesi gerekmeksizin, tek bir noktadan tüm hizmetlere erişebilmelerini sağlar ve kolayca değişen hizmetlerin adreslerini yönetmeyi kolaylaştırır.

Projede ocelot yapılandırmasını aşağıdaki gibi tanımladım:

  <code>Log</code>
```csharp
           {
  "Routes": [
    {
      "DownstreamPathTemplate": "/Api/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5000
        }
      ],
      "UpstreamPathTemplate": "/Api/{everything}",
      "UpstreamHttpMethod": [ "Get","Post","Delete","Put" ],
      "SwaggerKey": "Api"
    },
    {
      "DownstreamPathTemplate": "/Identity/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 3000
        }
      ],
      "UpstreamPathTemplate": "/Identity/{everything}",
      "UpstreamHttpMethod": [ "Get","Post","Delete","Put"],
      "SwaggerKey": "Identity"
    }
  ],

  "SwaggerEndPoints": [
    {
      "Key": "Identity",
      "Config": [
        {
          "Name": "Identity API",
          "Version": "v1",
          "Url": "http://localhost:3000/swagger/v1/swagger.json"
        }
      ]
    },
    {
      "Key": "Api",
      "Config": [
        {
          "Name": "Web API",
          "Version": "v1",
          "Url": "http://localhost:5000/swagger/v1/swagger.json"
        }
      ]
    },
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:1000"
  }
}

```

Web API katmanı 5000, Identity API katmanı 3000 portu olarak ayarlıyken swagger ortamında 1000 portu üzerinden işlemler başlatılmaktadır.

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/d2083ba9-1b77-40b5-af4d-d08bf4b73fdc)

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/38c8c481-44d9-406f-8919-c81cd722e44f)

# <h3 name="jwt-token"><strong>Json Web Token (JWT) Nedir<strong></h3>

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/843b1185-9339-4cc9-ad11-671bf24649a1)


JWT (JSON Web Token), kullanıcıların kimlik doğrulamasını sağlamak için kullanılan bir açık standarttır. Genellikle bir kullanıcının yetkilendirilmesini ve dijital olarak imzalanmış bilgilerin güvenli bir şekilde paylaşılmasını sağlamak için kullanılır. JWT'ler, JSON formatında verileri kodlamak ve güvenli bir şekilde taşımak için kullanılır.

Üye girişi yapılırken API üzerinde örnek Token çıktısı :

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/7f65c3a0-89cb-450f-abaa-d0b2cab2b789)


# <h3 name="refresh-token"><strong>Refresh Token<strong></h3>

Refresh token, asıl kimlik doğrulama süreci sonucunda elde edilen bir tür kimlik belgesidir. Bu belge, kullanıcının oturumunu açık tutma ve kimlik doğrulama süresi dolmuş olsa bile tekrar erişim sağlama amacı taşır. Genellikle belirli bir süre boyunca geçerlidir (örneğin, bir saat), ancak kısa süreli bir jeton (access token) ile karşılaştırıldığında daha uzun bir ömre sahiptir.

Access token, kullanıcının belirli kaynaklara (örneğin, API'lar) erişimini sağlamak için kullanılırken, refresh token kimlik doğrulama sunucusuna gidip yeni bir access token talep etmek için kullanılır. Bu sayede, kullanıcı oturumu açık tutulabilir ve oturum süresi sona erdiğinde kullanıcının tekrar kimlik doğrulama yapmasına gerek kalmadan erişimi sürdürülebilir.

# <h3 name="log"><strong>LOG<strong></h3>

Log (kayıt), bir sistemde veya uygulamada meydana gelen olayları, hataları, işlemleri ve diğer önemli bilgileri düzenli bir şekilde kaydetmek için kullanılan metin tabanlı bir dosyadır. Loglar, sistemlerin çalışma durumunu izlemek, hata ayıklamak, performansı değerlendirmek ve sorunları tespit etmek için önemli bir araçtır. Yazılım geliştiricileri ve sistem yöneticileri, logları inceleyerek sistem davranışını anlamak ve sorunları çözmek için önemli bilgiler elde ederler.

Loglar genellikle çeşitli düzeylerde (log seviyeleri) kaydedilir:

DEBUG: Hata ayıklama amaçlı detaylı bilgiler.
INFO: Bilgilendirici mesajlar ve önemli olaylar.
WARNING: Uyarı mesajları, dikkat edilmesi gereken durumlar.
ERROR: Hatalar ve beklenmeyen olaylar, ancak uygulama çalışmaya devam ediyor.
CRITICAL: Kritik hatalar, uygulama çökme noktasına gelmiş olabilir

# <h4 name="seri-log"><strong>Seri LOG<strong></h4>

Seri log (serilog), .NET platformunda popüler bir loglama kütüphanesidir. .NET Core, ASP.NET Core ve diğer .NET uygulamalarında kullanılarak logları kolayca yönetmeyi ve farklı hedeflere kaydetmeyi sağlar. Serilog, yapılandırması kolay ve performans açısından etkili bir loglama çözümüdür.

  <code>Log</code>
```csharp
            if (!validatePassword)
            {
                _logger.LogError("Kullanıcı adı veya şifre yanlış");
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect.GetCustomDisplayName() }
                });
            }
```


# <h4 name="seq"><strong>SEQ<strong></h4>

Seq Serilog'un entegre edilebileceği ve logların izlenebileceği bir log yönetim sistemi ve görselleştirme aracıdır. Seq, logları toplamak, indekslemek ve aramak için kullanılır ve kullanıcı dostu bir web arayüzü üzerinden logları izlemeyi ve analiz etmeyi sağlar. Seq, uygulamanın loglarını daha kolay anlamak ve sorunları çözmek için güçlü bir araçtır. Ayrıca, logların saklanması ve analizi için farklı filtreleme ve sorgulama seçenekleri sunar. Seq, logları yönetmek ve izlemek için sistem yöneticileri ve geliştiriciler arasında yaygın olarak kullanılan bir araçtır.

# <h3 name="quartz"><strong>QUARTZ / JOB<strong></h3>

Quartz.NET, C# dilinde geliştirilen popüler ve açık kaynaklı bir zamanlama (scheduling) kütüphanesidir. Bu kütüphane, belirli görevleri belirli zaman aralıklarında veya belirli bir takvime göre otomatik olarak çalıştırmak için kullanılır. "Quartz" adı, kum saati anlamına gelir ve zamanlamayla ilgili işlemleri hatırlatmak için kullanılmıştır.

Quartz.NET, özellikle tekrar eden görevleri zamanlanmış bir şekilde yürütmek için yaygın olarak kullanılır. Sistemlerde veritabanı yedeklemesi, veri işleme, e-posta gönderme, rapor oluşturma, hatırlatıcılar ve diğer periyodik görevlerin otomatik olarak çalıştırılmasında kullanılabilir.

Quartz.NET, esnek bir mimariye sahiptir ve işlerin zamanlanmasını ve yürütülmesini yönetmek için bir dizi özelliği destekler. Bazı temel kavramlar ve özellikleri şunlardır:

Job (İş): Yapılması gereken görevin temel birimini temsil eder. Gerçek işlemler, bu iş birimleri içinde tanımlanır.

Trigger (Tetikleyici): İşlerin ne zaman çalıştırılacağını belirten bileşenlerdir. Basit zamanlamalar (örneğin, belirli bir süre sonra çalıştırma) veya daha karmaşık ifadelerle (örneğin, belirli bir saatte, haftada bir, ayda bir vb. çalıştırma) zamanlamalar sağlayabilirler.

Scheduler (Zamanlayıcı): İşlerin ve tetikleyicilerin yönetildiği ana bileşendir. Zamanlayıcı, tanımlanan işleri ve tetikleyicileri izler ve ilgili zamanlarda işleri yürütür.

JobDataMap: İşlere ekstra verileri taşımak için kullanılır. Bu şekilde, işler çalıştırıldığında belirli parametreler veya veriler işlere iletilebilir.

Quartz.NET, aynı anda birden fazla işlemi destekler ve sistem üzerinde yüksek performans ve istikrar sağlamak için uygun şekilde tasarlanmıştır. Ayrıca, veritabanı desteği sayesinde zamanlama bilgileri kalıcı bir şekilde saklanabilir, bu da uygulama yeniden başlatıldığında zamanlanmış işlerin korunmasını sağlar.

Bu nedenle, Quartz.NET, periyodik işlerin ve görevlerin hatasız bir şekilde ve planlanan zamanlarda çalışmasını sağlamak için C# projelerinde oldukça kullanışlı ve tercih edilen bir zamanlama kütüphanesidir.

<code>Quartz / Job - Service Yapılandırması</code>
```csharp
public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        return services
            .AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                #region SendOtp

                var sendOtpJob = new JobKey("sendOtp");
                q.AddJob<SendOneTimePasswordJob>(opts => opts.WithIdentity(sendOtpJob));

                q.AddTrigger(opts => opts
                    .ForJob(sendOtpJob)
                    .WithIdentity("sendOtp-trigger")
                    .WithCronSchedule("0/1 * * * * ?"));

                #endregion
                
                
            }).AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
```

<code>Örnek Job Kullanımı - Belirli Aralıklarla Tek Kullanımlık Şifrenin SMS/Mail Olarak İletilmesini Tetikleyen Fonksiyon</code>
```csharp
[DisallowConcurrentExecution]
public class SendOneTimePasswordJob : IJob
{
    private readonly IMessageServiceFactory _messageServiceFactory;
    private readonly ITwoFactorAuthenticationRepository _twoFactorAuthenticationRepository;

    public SendOneTimePasswordJob(IMessageServiceFactory messageServiceFactory,
        ITwoFactorAuthenticationRepository twoFactorAuthenticationRepository)
    {
        _messageServiceFactory = messageServiceFactory;
        _twoFactorAuthenticationRepository = twoFactorAuthenticationRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var oneTimePasswords = await
            _twoFactorAuthenticationRepository
                .Query()
                .Where(_ => _.IsSend == false)
                .ToListAsync();

        foreach (var oneTimePassword in oneTimePasswords)
        {
            var factory = _messageServiceFactory.MessageService(oneTimePassword.OneTimePasswordChannel);

            await factory.SendMessage(oneTimePassword.To, Subject(oneTimePassword.OneTimePasswordType),
                oneTimePassword.OneTimePassword);

            oneTimePassword.IsSend = true;
            _twoFactorAuthenticationRepository.Update(oneTimePassword);
            await _twoFactorAuthenticationRepository.SaveChangesAsync();
        }
    }

    string Subject(OneTimePasswordType oneTimePasswordType)
    {
        return oneTimePasswordType switch
        {
            OneTimePasswordType.Register => "Koda Dair - Kayıt Aşamasına Son Adımdasınız!",
            OneTimePasswordType.ForgotPassword => "Koda Dair - Şifremi Unuttum",
            _ => throw new Exception("Tanımsız")
        };
    }
}
```
# <h3 name="rabbitmq"><strong>RabbitMQ<strong></h3>

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/8d38e554-7721-4986-97e5-73723f571294)


RabbitMQ, açık kaynaklı ve çoklu protokol destekli bir mesaj kuyruğu (message queue) yazılımıdır. Mesaj kuyrukları, dağıtık sistemlerde ve uygulamalarda mesajların asenkron olarak iletilmesini ve işlenmesini sağlayan araçlardır. Bu, sistemlerin daha esnek, ölçeklenebilir ve birbirinden bağımsız çalışmasına olanak tanır.

RabbitMQ, mesaj kuyruklarının temel prensiplerine dayalı olarak çalışır. Bir mesaj gönderici, mesajları belirli bir kuyruğa gönderir ve bu kuyruktan alıcı tarafından alınması beklenir. Mesajlar, kuyruklarda bekletilir ve alıcılar bu kuyruklardan mesajları alarak işleme alırlar.

RabbitMQ'nun temel terimleri şunlardır:

Producer (Üretici): Mesajları kuyruğa gönderen tarafı temsil eder. Bu, genellikle bir uygulama veya sistem bileşeni olabilir.

Queue (Kuyruk): Mesajların geçici olarak depolandığı ve beklediği yapıdır. Alıcılar mesajları bu kuyruklardan alır ve işleme alır.

Consumer (Tüketici): Kuyruklardan mesajları alıp işleyen tarafı temsil eder. Bu, mesajları alan ve uygun işlemi gerçekleştiren uygulama veya sistem bileşenidir.

Exchange (Değişim): Üreticilerin mesajları kuyruklara gönderirken belirli kurallara göre yönlendirmesini sağlar. Alıcılar, mesajları belirli bir değişime bağlanarak alırlar.

  <code>Rabbit MQ</code>
```csharp
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("test-url");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("kuyruk-mesaj", true, false, false);

            var body = Encoding.UTF8.GetBytes("Deneme mesaj");
            
            channel.BasicPublish(String.Empty, "kuyruk-mesaj", null, body);
            
            return await Result<object>
                .SuccessAsync("");
```

# <h3 name="exception-middleware"><strong>Exception Middleware<strong></h3>

Web uygulamaları kullanıcıların etkileşimde bulunduğu ve çeşitli isteklerin işlendiği platformlardır. Bu isteklerin işlenmesi sırasında hatalar meydana gelebilir. Örneğin, veritabanına erişim hatası, geçersiz istek formatı, yetkilendirme sorunları vb. Bu hataların kullanıcıya anlaşılır bir şekilde sunulması, uygulamanın güvenliğinin sağlanması ve sorunların hızla tespit edilmesi gereklidir.

Exception middleware (istisna aracı), bu tür hataları yönetmek için kullanılır. Web uygulamasının işlem süreci sırasında meydana gelen istisnaları (hataları) yakalar ve bu istisnaları daha anlamlı ve kullanıcı dostu hata mesajlarına dönüştürerek kullanıcıya sunar. Aynı zamanda, uygulama geliştiricilerine de hata izleme ve sorun giderme konusunda yardımcı olur.

Bu ara katman, uygulama kodunun merkezi bir noktasında yer alır ve istisna durumlarına odaklanır. Genellikle istisna yönetimine ve hata mesajlarının yönetilmesine dair kod tekrarını azaltmak ve kod tabanını daha düzenli hale getirmek için kullanılır.

Bu şekilde, kullanıcılar daha iyi hata mesajları alırken, geliştiriciler de hataları daha etkili bir şekilde izleyebilir ve çözebilir. Bu da uygulamanın kalitesini artırır ve daha iyi bir kullanıcı deneyimi sağlar.

<code>Exception Middleware Kod Yapısı</code>
```csharp
 public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            ErrorResult errorResult = new ErrorResult()
            {
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
            };
            
            errorResult.Messages!.Add(exception.Message);
            var response = context.Response;
            response.ContentType = "application/json";
            
            

            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }
            
            switch (exception)
            {
                case UserFriendlyException e:
                    errorResult.ErrorCode = Convert.ToInt32(e.ExceptionTypeEnum);
                    errorResult.ErrorMessage = e.ExceptionTypeEnum.GetAttribute<DisplayAttribute>()?.Name;
                    errorResult.StatusCode = response.StatusCode = e.ExceptionTypeEnum.GetAttribute<DisplayAttribute>().Order;
                    errorResult.Messages = e.ErrorMessages;
                    break;
                case CustomException e:
                    response.StatusCode = errorResult.StatusCode = (int) e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                default:
                    response.StatusCode = errorResult.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }
            
            string json = JsonSerializer.Serialize(errorResult);
            await response.WriteAsync(json);
        }
    }
}
```

# <h3 name="AutoMapper"><strong>Auto Mapper<strong></h3>

![resim](https://github.com/Yuemwrite/BaseProject-KodaDair/assets/32547627/11470c03-96d9-48c2-848d-cfc14cdcd02f)

AutoMapper, bir nesne eşlemesi (object mapping) kütüphanesidir ve genellikle bir veri modelini başka bir veri modeline dönüştürmek için kullanılır. Özellikle yazılım geliştirme projelerinde veritabanı varlıklarını, veri transfer nesnelerini (DTO'lar) veya görüntü modellerini dönüştürmek için tercih edilen bir araçtır.

AutoMapper Neden Kullanılmalı?

1 - Kod Tekrarını Azaltma: İki farklı veri modeli arasında dönüşüm yapmak gerektiğinde, bu dönüşümü tekrar tekrar yazmak yerine AutoMapper kullanarak kod tekrarını en aza indirebilirsiniz.

2 - Veri Modeli Ayırma: Veritabanı varlıkları genellikle iş mantığına sahip nesneleri yansıtırken, kullanıcı arabirimine gösterilen nesneler (görüntü modelleri veya DTO'lar) sadece görüntülemek amacıyla kullanılır. AutoMapper bu iki tür nesne arasındaki dönüşümü kolaylaştırır.

3 - Sürdürülebilirlik ve Bakım: Uygulama gereksinimleri değiştikçe veya veri modelleri güncellendikçe, dönüşüm mantığını güncellemek veya ayarlamak daha kolaydır. AutoMapper sayesinde, dönüşüm mantığını tek bir yerde güncellemek yeterlidir.

4 - Performans İyileştirmeleri: AutoMapper, dönüşümleri optimize ederek performansı artırabilir. Özellikle büyük veri kümesi üzerinde çalışırken, manuel dönüşümler yerine AutoMapper kullanmak performans açısından avantajlı olabilir.


<code>AutoMapper</code>
```csharp
            reply.Content = reply.Content;
            reply.RowVersion = reply.RowVersion;

            _repository.Update(reply);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ReplyDto>(reply);
```

Yukarıdaki kod örneğinde reply adındaki entity AutoMapper Dependency Injection kullanılarak ReplyDto modeline dönüştürülmektedir.

# <h3 name="seed-yapi"><strong>Seed Yapısı<strong></h3>

Seed data, bir uygulamanın başlangıcında veritabanına önceden tanımlanmış verileri eklemek için kullanılır. Bu veriler, uygulama geliştirilirken veya test edilirken kullanılacak başlangıç ​​verilerini temsil eder.

Seed data'nın kullanımı özellikle geliştirme ve test amaçları için faydalıdır. Uygulamanızı geliştirirken, veritabanınızın nasıl çalışacağını ve görüneceğini anlamak için gerçekçi verilere ihtiyaç duyarsınız. Seed data, bu veritabanı işlemlerini test etmek ve görüntülemek için başlangıçta kullanılabilecek örnek verileri sağlar. Böylece uygulamanızın veritabanı katmanını test etmek ve geliştirmek daha kolay hale gelir.

Projede gönderi paylaşımı için gerekli olan kategori ve alt kategorilere ait belli başlı verileri uygulama ilk ayağa kalkarken seed veri olacak şekilde ayarladım.

<code>Seed Data / Kategori Verileri</code>
```csharp
public class CategorySeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;

    public CategorySeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        if (_db.Categories.Any()) return;
        _db.Categories.AddRange(
            new Category()
            {
                Name = "Backend",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new Category()
            {
                Name = "Frontend",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new Category()
            {
                Name = "Mobile",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new Category()
            {
                Name = "Game",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            });
        _db.SaveChanges();
    }
}
```

<code>Seed Data / Alt Kategori Verileri</code>
```csharp
public class SubCategorySeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;

    public SubCategorySeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        if (_db.SubCategories.Any()) return;
        _db.SubCategories.AddRange(
            new SubCategory()
            {
                Name = "C#",
                CategoryId = _db.Categories.First(_ => _.Name =="Backend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Java",
                CategoryId = _db.Categories.First(_ => _.Name =="Backend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Python",
                CategoryId = _db.Categories.First(_ => _.Name =="Backend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Vue.JS",
                CategoryId = _db.Categories.First(_ => _.Name =="Frontend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "React.JS",
                CategoryId = _db.Categories.First(_ => _.Name =="Frontend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Angular.JS",
                CategoryId = _db.Categories.First(_ => _.Name =="Frontend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Flutter",
                CategoryId = _db.Categories.First(_ => _.Name =="Mobile").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "React Native",
                CategoryId = _db.Categories.First(_ => _.Name =="Mobile").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Unity",
                CategoryId = _db.Categories.First(_ => _.Name =="Game").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            });
        _db.SaveChanges();
    }
}
```

# <h3 name="pagination"><strong>Sayfalama Yapısısı(Pagination)<strong></h3>

Pagination (sayfalama), bir veri kümesini daha küçük ve daha yönetilebilir parçalara bölmek ve bu parçaları ayrı ayrı sayfalar halinde göstermektir. Bu genellikle uzun veya büyük veri listelerini veya sonuç kümesini kullanıcılar için daha erişilebilir hale getirmek amacıyla kullanılır.


Pagination genellikle aşağıdaki bileşenleri içerir:

 - Sayfa Numaralandırması: Kullanıcıya hangi sayfada olduklarını ve kaç sayfa olduğunu gösteren sayfa numaraları veya sayfa ilerleme düğmeleri sağlar.

 - Öğe Sayısı: Bir sayfada kaç öğenin görüntüleneceğini belirten bir seçenek sağlar. Örneğin, bir sayfada 10 veya 20 öğe gösterilebilir.

 - Önceki ve Sonraki Düğmeleri: Kullanıcıların bir sonraki veya önceki sayfaya geçmelerini sağlayan düğmelerdir.

 - İlk ve Son Sayfa Düğmeleri: Kullanıcıların ilk veya son sayfaya hızla gitmelerine olanak tanır.

 - Sayfa Aralığı Sınırlamaları: Kullanıcının aynı anda görüntülenebilecek maksimum sayfa sayısını sınırlar. Örneğin, 1-10 arası sayfaları göster gibi.


<code>Sayfalama Yapısı Örnek Kullanım</code>
```csharp
          var reply = _replyRepository
                .Query()
                .Include(_ => _.Comment)
                .Include(_ => _.Likes)
                .Include(_ => _.User)
                .Where(_ => _.CommentId == request.CommentId);

            var replyQuery = reply
                .Select(_ => new ReplyDto()
                {
                    Id = _.Id,
                    CommentId = _.CommentId,
                    UserId = _.UserId,
                    UserName = _.User.UserName,
                    Content = _.Content,
                    CreationTime = _.CreationTime,
                    LikeCount = _.Likes.Where(_=>_.IsActive).Count(like => true),
                    RowVersion = _.RowVersion
                }).AsNoTracking();

            var result = await _replyRepository.GetPagedResult(replyQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: rply => rply.OrderByDescending(_ => _.CreationTime),
                cancellationToken: cancellationToken);

            return HandleResult(result);
```

# <h3 name="environment"><strong>Environment Yapısısı<strong></h3>

Projede Local ve Production olmak üzere 2 farklı environment (ortam) yer almaktadır. Projenin ayağa kalkması için infrastructure altındaki Configurations klasöründe yer alan local ve production ortamlarında temel ayarlamaların yapılması gerekir. (Infrastructure, API katmanlarıyla bağlantılı çalışmaktadır. Ayarlamaların Infrastructure katmanında yapılması yeterli olacaktır.)

<code>Environment'e Göre / AppSettings.json Ayarlamaları</code>
```csharp
          {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "SqlServer": ""
  },
  "AllowedHosts": "*",
  "JwtSettings": {
    "ValidAudience" : "test",
    "ValidIssuer": "test",
    "Key": "",
    "TokenExpirationInMinutes": 15,
    "RefreshTokenExpirationInDays": 7,
    "AllowConcurrentSessions": true
  },
  "RedisConfiguration": {
    "Host": "localhost",
    "Port": 6379
  },
  "EmailConfiguration" : {
    "IsEnabled" : true,
    "Host" : "",
    "Port" : 587,
    "Username" : "",
    "Password" : "",
    "SenderEmil" : "",
    "SenderName" : ""
  }
}
```

Migration Çıkmak ve Database Oluşturmak İçin Terminalde Aşağıdaki Kodların, Environment'e göre, Çalıştırılması Gerekir

<code>Environment Seçimi</code>
```csharp
   $Env:ASPNETCORE_ENVIRONMENT = "Local"
```

<code>Migration Ekleme</code>
```csharp
   dotnet ef migrations add Db_v1 -p Migratiors.Local --context ApplicationDbContext -o Migrations -s BaseProject.WebAPI
```

<code>Database Güncelleme</code>
```csharp
   dotnet ef database update -p BaseProject.WebAPI -c ApplicationDbContext 
```

### <h1 name="koda-dair"><strong>KodaDair<strong></h1>

Alt yapı işlemlerini tamamladıktan sonra alt yapıda yer verdiğim teknolojileri kullanmak üzere bir proje senaryosu düşündüm ve projenin adına KodaDair ismini verdim. KodaDair projesine ilişkin detaylı bilgiler alt kısımlarda yer almaktadır.

   * <h3 name="kodadair-konu"><strong>Proje Ana Konusu<strong></h3>
     
KodaDair, yazılıma dair her şeyin paylaşıldığı dinamik bir sosyal medya platformunu sunuyor. Yazılım geliştiricilerini, bilgisayar/yazılım mühendislerini ve kod yazmayı bir tutku haline getiren herkesi büyülü bir sosyal medya platformunda bir araya getiriyor. Adıyla da örtüşen bu platform, yazılımın kilit noktalarına odaklanarak, kullanıcılarına sadece sorular sorma ve yanıtlama olanağı sunmakla kalmıyor, aynı zamanda paylaşılan fikirlerin filizlenmesine, sorunların derinlemesine keşfine ve yaratıcı çözümlerin ortaya çıkmasına zemin hazırlıyor.

KodaDair, katılımcılarına çeşitli kategoriler altında derinlemesine sorular yöneltme, kendi deneyimlerini paylaşma ve topluluğun zengin bilgi havuzuna katkıda bulunma fırsatı sunarak, yazılım dünyasının sınırlarını genişletiyor. Burada, kodların dilinden daha fazlasını paylaşan bir toplulukla karşılaşacaksınız; paylaşılan her fikir, cevap ve yorum, kolektif bir akıl yürütmenin parçası olarak değer kazanıyor.

KodaDair, karmaşıklığın ötesine geçerek, yazılımın aslında bir iletişim ve işbirliği sanatı olduğunu vurguluyor. Bu platform, kodların ardındaki hikayeleri anlatma, problem çözme yeteneklerini geliştirme ve yazılım dünyasındaki deneyimleri paylaşma amacını taşıyor. KodaDair ile, yazılımın büyülü evreninde gezinirken sadece bilgiye değil, aynı zamanda insanların birbirleriyle olan bağları da güçlenecektir.

# <h3 name="kodadair-modul"><strong>KodaDair Yer Verilen Modüller<strong></h3>
   
 - <strong>İki Aşamalı Kayıt Ol Sistemi<strong>

KodaDair sistemine kayıt ol sistemi iki aşamalı olarak gerçekleştirilmektedir. Kayıt ol aşamasında alınan email adresine gönderilen tek kullanımlık şifre ile email adresi onaylanarak kullanıcı kayıt işlemi tamamlanır.

 - <strong>Sisteme Giriş Yap</strong>

Kaydı tamamlanan kullanıcılar kayıt aşamasındaki bilgileri ile (kullanıcı adı ve şifre) sisteme giriş yapabilir. Json Web Token ve Refresh token yapısına projede yer verilmiştir.

 - <strong>Şifremi Unuttum Akışı<strong>

KodaDair platformunda her an başınıza gelebilecek bir durumu düşünerek, şifrenizi unutsanız bile endişelenmeyin! Kullanıcı adınızı girip email ya da sms kanal seçimi de yaparak (şuan için projede sms entegrasyonu olmadığı için email üzerinden işlemler gerçekleştirilmektedir.) alınan tek kullanımlık şifre ile birlikte yeni şifre belirleme işlemi gerçekleştirilir.

 - <strong>Gönderi Paylaşımı<strong>

Yazılım dünyasındaki keşiflerinizi ve deneyimlerinizi paylaşmanız için mükemmel bir alan. Gönderi Paylaşımı modülüyle ilham veren yazılarınızı ve projelerinizi platformda sergileyebilirsiniz. Ayrıca karşılaştığınız herhangi bir yazılım programı ve yazılım dili ile alakalı kategoriler/alt kategoriler altında sorularınızı sorabilir, cevap arayabilir ya da sorulan sorulara cevaplar verebilirsiniz.

Ayrıca profilinize giren diğer kullanıcıların en üstte görmesini istediğiniz gönderinizi sabitleme özelliği de yer alır. Bu sayede profilinizde istediğiniz gönderiyi sabitleyerek listede en tepeye gelmesini sağlayabilirsiniz.

- <strong>Gönderilere Yorum Yapma<strong>

Paylaşılan gönderilere yorum yaparak görüşlerinizi ifade edebilir, deneyimlerinizi paylaşabilir ve topluluğunuzla etkileşimde bulunabilirsiniz.

- <strong>Yapılan Yorumlara Yanıt Verme<strong>

Sadece yorum yapmakla kalmayın, aynı zamanda diğer kullanıcıların yorumlarına cevap vererek fikir alışverişinde bulunun, yeni bağlantılar kurun.

- <strong>Beğeni Sistemi<strong>

Hoşunuza giden, beğendiğiniz gönderileri, cevapları veya yanıtları beğenerek gönderiye etkileşim kazandırabilirsiniz. Beğendiğiniz gönderileri ve yorumları özgürce işaretleyerek ilgi ve takdirinizi gösterebilirsiniz. Aynı şekilde, beğenmekten vazgeçme seçeneğiyle de görüşlerinizi güncelleyebilirsiniz.

- <strong>Kullanıcı Takip Et/Takipten Çıkar<strong>

Yazılım dünyasının ilham veren figürlerini takip ederek en yeni güncellemeleri kaçırmayın. Aynı zamanda istediğiniz zaman takipten çıkarak içeriği kişiselleştirin. 

- <strong>Kullanıcı Profil Modülü<strong>

Kendinizi en iyi şekilde ifade etmek için kişisel profilinizi özelleştirin. Deneyimlerinizi ve projelerinizi paylaşarak kendinizi tanıtın.

- <strong>Kullanıcı Eğitim Bilgileri<strong>

Eğitim geçmişinizi paylaşarak akademik yolculuğunuzu topluluğa aktarın. Bu modül, bilgi alışverişini daha anlamlı hale getirmenize yardımcı olur.

- <strong>Kullanıcı İş Deneyimleri<strong>

Yazılım dünyasındaki iş deneyimlerinizi paylaşarak kariyer yolculuğunuzu ve uzmanlığınızı gösterin. Bu modül, diğer kullanıcılarla bağlantı kurmanıza ve mentorluk ilişkileri oluşturmanıza olanak tanır.
 
- <strong>Hesabı Gizliye Alma<strong>

Hesap Gizliye Alma modülü sayesinde içerik yalnızca belirlenen izleyicilere sunulabilir. Bu şekilde, sizi takip etmek isteyen kullanıcılar sadece siz onların takip isteklerini kabul ettiğinizde takibe başlayabilirler ve gönderilerinizi görebilirler.

- <strong>Gönderi Kaydet/Favorilere Ekle<strong>

Kullanıcılar, ilham verici veya önemli buldukları gönderileri kolayca favorilerine ekleyebilir, böylece bu içeriklere daha sonra kolayca erişebilirler. Kullanıcılar, unutulmaması gereken kod örnekleri, etkileyici proje paylaşımları veya ilgi çekici yazılar gibi değerli içerikleri kaydedebilir ve bu şekilde kendileri için anlamlı bir koleksiyon oluşturabilirler. Bu modül, yazılım tutkunlarının ilgi çekici içerikleri saklama ve paylaşma deneyimini daha kişisel hale getirerek KodaDair'in özgünlüğünü bir adım öne taşıyor.

# <h3 name="kodadair-hedef"><strong>KodaDair Hedeflenen Modüller<strong></h3>

- Bildirim Modülü
- Anket Sistemi
- Kullanıcı engelleme
- Rozet Sistemi
- Puanlama sistemi
- Yetkilendirme sistemi
  
# <h3 name="kodadair-api"><strong>KodaDair Kullanılan API'ler<strong></h3>

 Projede IdentityApi ve WebApi olmak üzere 2 farklı Presentation katmanı bulunmaktadır. 
 
 IdentityApi katmanında kullanıcının sisteme giriş yaptığı ve şifremi unuttum akışlarının olduğu API'ler yer almaktadır.

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/37078950-06e7-4d2d-8940-af0ea0c301d6)


 WebApi katmanında ise tüm kullanıcı işlerinin gerçekleştirildiği Api'ler yer almaktadır. Sırasıyla:

 - Sisteme Kayıt Yapan API'ler

![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/a158358f-40d5-4d3c-85da-4865318c97eb)


- Eğitim Bilgilerinin Yönetildiği API'ler

 ![resim](https://github.com/Yuemwrite/BaseProject-KodaDair/assets/32547627/64521db9-790d-4065-9c0a-03f37c990fa3)


- İş Deneyimi Bilgilerinin Yönetildiği API'ler

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/4fdaf3f8-4872-44dd-ae9f-405630df9c71)

- Kullanıcı Profili Bilgilerinin Yönetildiği API'ler

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/53d3d2c1-8ea7-4da8-a329-6fa5b3f8a7ff)

- Kullanıcı Takip/Takipçi İsteklerini ve Hesap Gizliliğini Yönetildiği API'ler

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/c3f4d721-ff9a-4288-8097-59f77d2b6ef2)

- Gönderi İşlemlerinin Yönetildiği API'ler

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/48f1ee86-cc02-4c49-a58e-3dc86e30d52b)


- Yorum / Yanıt / Beğeni İşlemlerinin Yönetildiği API'ler

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/6870b456-8e2b-4ac4-b77c-b95e1f54a91e)

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/582dce64-c2bd-42c2-a316-d7fafcdc6e03)

 ![resim](https://github.com/Yuemwrite/BaseProject/assets/32547627/b2b343f4-015f-40a0-93a7-5cbb0e89e1ee)

- Admin İşlemlerinin Yönetildiği API'ler

![resim](https://github.com/Yuemwrite/BaseProject-KodaDair/assets/32547627/7c567551-5719-4a9a-9a4b-0bc9256beec2)


# <h1 name="diger"><strong>Projeye İlişkin Diğer Bilgiler<strong></h1>

 - Tercih Edilen IDE: JetBrains Rider 2023.2
 - Kullanılan Versiyon: .NET 7
 - Programlama Dili ve Teknolojiler: C#, .Net Core, Web API Projesi

# <h1 name="kapanis"><strong>Kapanış<strong></h1>

.NET Core alanında öğrendiklerimi harmanlamaya ve alt yapısal olarak birçok konuya değinmeye çalıştığım projemi sizlere de aktarmak istedim. Genel hatlarıyla bahsettiğim projenin içeriğini yine bu repo üzerinden geliştirmeye devam edeceğim. Sizler de proje hakkındaki düşünce ve fikirlerinizi benimle paylaşmak, projeye destek olmak isterseniz memnuniyet duyarım. Kaynağın sizlere bol fayda sağlaması dileğiyle ✋🎉

# <h1 name="kaynakca"><strong>Kaynakçalar<strong></h1>

  - [https://yunusemrehaslak.com/cqrs-pattern-ile-repository-pattern-kullanimi/](https://yunusemrehaslak.com/cqrs-pattern-ile-repository-pattern-kullanimi/)
  - [https://www.youtube.com/watch?v=Vgpqts9qkhE&list=PLQVXoXFVVtp2eAq33DVNxeoXLXj4VMYpT](https://www.youtube.com/watch?v=Vgpqts9qkhE&list=PLQVXoXFVVtp2eAq33DVNxeoXLXj4VMYpT)
  - [https://www.gencayyildiz.com/blog/category/redis/](https://www.gencayyildiz.com/blog/category/redis/)
  - [https://yunusemrehaslak.com/api-gateway-nedir/](https://yunusemrehaslak.com/api-gateway-nedir/)
  - [https://www.youtube.com/watch?v=UI5jFgr2vNw](https://www.youtube.com/watch?v=UI5jFgr2vNw)
  - [https://www.youtube.com/watch?v=CAQRpuVInq0](https://www.youtube.com/watch?v=CAQRpuVInq0)
  - [http://blog.alicancevik.com/net-core-automapper-kullanimi/](http://blog.alicancevik.com/net-core-automapper-kullanimi/)
  - [https://www.youtube.com/watch?v=Vgpqts9qkhE&list=PLQVXoXFVVtp2eAq33DVNxeoXLXj4VMYpT](https://www.youtube.com/watch?v=Vgpqts9qkhE&list=PLQVXoXFVVtp2eAq33DVNxeoXLXj4VMYpT)
  - [https://www.gencayyildiz.com/blog/category/redis/](https://www.gencayyildiz.com/blog/category/redis/)

  
    
    
    

 


 

 


