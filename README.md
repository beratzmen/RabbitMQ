<h1>Rabbitmq öğrenirken almış olduğum notları ve kod örneklerini içerir.</h1>

<h2>Publisher and Subscriber Projects</h2>
<p>
   <h4>Bu yöntemde publisher, mesajları doğrudan kuyruğa gönderir ve subscriber'lar bu kuyruğa bağlanarak mesajları tüketir.</h4>
      
   <i>1 publisher'ın 1.5 saniye aralıklarla queue ismindeki rabbitmq kuyruğuna mesajı göndermesi ve 2 subscriber'ın sırayla bu kuyruktaki mesajları okuduğu örnek çalışma videosunu aşağıdaki play butonuna tıklayarak izleyebilirsiniz.</i>
</p>

<h2>PublisherForFanoutExchange and SubscriberForFanoutExchange Projects</h2>
<p>
   <h4>Fanout Exchange yöntemi, bu yöntemde publisher mesajları exchange gönderir. Kuyruk oluşturulmamaktadır. Her consumer kendi kuyruğunu oluşturarak mevcut exchange bağlanır ve mesajları tüketir. Her subscriber'a exchange'deki mesajlarda eşit olarak iletilmektedir.</h4>
  
  <i>Publisher 1.5 saniye aralıklarla <b>logs</b> isminde exchange mesajları gönderir. Kuyruk oluşturulmamıştır. Eğer <b>client(subscriber)</b> yoksa veya kuyruk oluşturup exchange bağlanmazsa mesajlar silinir. SubscriberForFanoutExchange projesi içerisinde <b>randomQueueName</b> ile her client için birbirinden farklı kuyruk oluşturup exchange bağlanılmıştır. Subscriber'lar aynı exchangedeki aynı verileri tüketmektedir.</i>
</p>

<h2>PublisherForDirectExchange and SubscriberForDirectExchange Projects</h2>
<p>
  <h4>Direct Exchange yöntemi, route keyword'üne göre aldığı mesajları kuyruklara iletmektedir. Daha sonra her bir subscriber ilgili kuyruğa bağlanarak mesajları tüketmektedir.</h4>
  
  <i>Publisher içerisinde <b>Critical, Error, Warning, Information</b> 4 çeşit hata tipi tanımlanmıştır. Bu hata tipleri <b>logs-direct</b> exchange'ine <b>direct-queue-{}</b> hata ismiyle beraber, kuyruk ismini alarak kuyruğu oluşturur.</i>
</p>

<h2>PublisherForTopicExchange and SubscriberForTopicExchange Projects</h2>
<p>
  <h4>Direct Exchange yöntemi, daha spesifik routeKeyler oluşturmamıza yarar. Örnek routeKey: A.B.C olsun. 'A.#' dersek, kuyruğumuz A ile başlasın devamındaki karakterler umrumda değil demektir. Ya da '*.B.*' dersek, kuyruğumuzun ortasındaki karakterin B olması bizim için yeterli demektir.</h4>
  
  <i>Subscriber'daki örnek kodlar '*.Error.*' kuyruğuna bağlanacak şekilde ayarlanmıştır. Publisher'dan yayınlanan Critical.Error.Information şeklindeki routKey'lerden ortasında Error olan kuyruğa bağlanır ve oradaki verileri tüketir.</i>
</p>

<h2>PublisherForHeaderExchange and SubscriberForHeaderExchange Projects</h2>
<p>
  <h4>Bu yöntemde, kuyruğu route'lama yöntemi 'Header' bilgisiyle 'Dictionary' şeklinde gönderilmektedir. Ayrıca kuyruğa mesaj 'Class' instance olarak da gönderilmektedir.</h4>
  
  <i>Örnek olarak, publisher'dan header-exchange'ine route bilgisi olarak, 'format-pdf', 'shape-a4' şeklinde bir dictionary gönderilmiştir. Subscriber tarafından ise aynı isimler verilerek ilgili kuyruğa bağlanılmıştır. 'x-match' key'ine 'All' parametresi verilirse tüm değerlerin eşleşmesi gerekir(Publisher). All yerine 'Any' verilirse, eşleşen herhangi biri alınır</i>
</p>

<br/>
*Rabbitmq DOCKER üzerinde ayağa kaldırılmıştır.*

<hr/>
<br/>

https://user-images.githubusercontent.com/30552914/167259775-11a9db2b-24d1-48ed-b5bc-c2e3e95ad1ad.MP4
