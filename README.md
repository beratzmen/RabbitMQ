<h2>RabbitMQ</h2>
<h2>Rabbitmq öğrenirken almış olduğum notları ve kod örneklerini içerir.</h2>

<h3>Publisher and Subscriber Projects</h3>
<p><i>1 publisher'ın 1.5 saniye aralıklarla queue ismindeki rabbitmq kuyruğuna mesajı göndermesi ve 2 subscriber'ın sırayla bu kuyruktaki mesajları okuduğu örnek çalışma videosunu aşağıdaki play butonuna tıklayarak izleyebilirsiniz.</i></p>

<h3>PublisherForFanoutExchange and SubscriberForFanoutExchange Projects</h3>
<p>
   <h3>Fanout Exchange yöntemi, bu yöntemde publisher mesajları exchange gönderir. Kuyruk oluşturulmamaktadır. Her consumer kendi kuyruğunu oluşturarak mevcut exchange bağlanır ve mesajları tüketir. Her subscriber'a exchange'deki mesajlarda eşit olarak iletilmektedir.</h3>
  
  <i>Publisher 1.5 saniye aralıklarla <b>logs</b> isminde exchange mesajları gönderir. Kuyruk oluşturulmamıştır. Eğer <b>client(subscriber)</b> yoksa veya kuyruk oluşturup exchange bağlanmazsa mesajlar silinir. SubscriberForFanoutExchange projesi içerisinde <b>randomQueueName</b> ile her client için birbirinden farklı kuyruk oluşturup exchange bağlanılmıştır. Subscriber'lar aynı exchangedeki aynı verileri tüketmektedir.</i>
</p>

<h3>PublisherForDirectExchange and SubscriberForDirectExchange Projects</h3>
<p>
  <h3>Direct Exchange yöntemi, route keyword'üne göre aldığı mesajları kuyruklara iletmektedir. Daha sonra her bir subscriber ilgili kuyruğa bağlanarak mesajları tüketmektedir.</h3>
  
  <i>Publisher içerisinde <b>Critical, Error, Warning, Information</b> 4 çeşit hata tipi tanımlanmıştır. Bu hata tipleri <b>logs-direct</b> exchange'ine <b>direct-queue-{}</b> hata ismiyle beraber, kuyruk ismini alarak kuyruğu oluşturur.
  </i>
</p>


<br/>
*Rabbitmq DOCKER üzerinde ayağa kaldırılmıştır.*

<hr/>
<br/>

https://user-images.githubusercontent.com/30552914/167259775-11a9db2b-24d1-48ed-b5bc-c2e3e95ad1ad.MP4
