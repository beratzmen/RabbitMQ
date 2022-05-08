<h2>RabbitMQ</h2>
<h2>Rabbitmq öğrenirken almış olduğum notları ve kod örneklerini içerir.</h2>

<h3>Publisher and Subscriber Projects</h3>
<p><i>1 publisher'ın 1.5 saniye aralıklarla queue ismindeki rabbitmq kuyruğuna mesajı göndermesi ve 2 subscriber'ın sırayla bu kuyruktaki mesajları okuduğu örnek çalışma videosunu aşağıdaki play butonuna tıklayarak izleyebilirsiniz.</i></p>

<h3>PublisherForFanoutExchange and SubscriberForFanoutExchange Projects</h3>
<p><i>Publisher 1.5 saniye aralıklarla <b>logs</b> isminde exchange mesajları gönderir. Kuyruk oluşturulmamıştır. Eğer <b>client(subscriber)</b> yoksa veya kuyruk oluşturup exchange bağlanmazsa mesajlar silinir. SubscriberForFanoutExchange projesi içerisinde <b>randomQueueName</b> ile her client için birbirinden farklı kuyruk oluşturup exchange bağlanılmıştır. Subscriber'lar aynı exchangedeki aynı verileri tüketmektedir.</i></p>


<br/>
*Rabbitmq DOCKER üzerinde ayağa kaldırılmıştır.*

<hr/>
<br/>

https://user-images.githubusercontent.com/30552914/167259775-11a9db2b-24d1-48ed-b5bc-c2e3e95ad1ad.MP4
