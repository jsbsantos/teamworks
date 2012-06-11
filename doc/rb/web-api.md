Web Api 
=

De forma a disponibilizar aos utilizadores uma forma de interagir e integrar aplicações externar com a infra-estrutura foi desenvolvida um serviço web disponibilizado através de uma api web. **(confuso?)**

A web api assenta sobre modelo de arquitectura ReST ^[Representational state transfer] e a comunicação é feita utilizando o protocolo HTTP. 
Neste modelo todos os objectos da aplicação são considerados recursos e têm um URL único. 
Como referência para a implementação do protocolo HTTP foi usado o documento RFC2616 - "Hypertext Transfer Protocol - HTTP/1.1" ^[http://www.w3.org/Protocols/rfc2616/rfc2616.html].

Unit of Work
-

Cada pedido feito à api web necessita, usualmente, de aceder ao repositório de dados, RavenDB. Esse acesso é feito através da criação de uma Sessão para acesso ao RavenDB que, como foi dito no capítulo anterior, implementa o padrão Unit of Work.

A Sessão tem o tempo de vida de um pedido HTTP, é criada durante o processamento do pedido e no final do pedido as alterações feitas à Sessão são persistidas na base de dados.
A persistência é feita depois da *action* correspondente ser executada pela classe *RavenHandler* registada no *pipeline* HTTP durante o arranque da api. Esta classe verifica se o pedido foi realizado com sucesso e persiste as alterações usando o código seguinte.

````
using (var session = Global.Raven.CurrentSession) {  
    if (session != null && t.Result.IsSuccessStatusCode)  {  
        session.SaveChanges();  
    }  
}  
````

Caso durante o processamento do pedido ocorra um erro e o código da resposta não for de sucesso as alterações não são persistidas, desta forma um pedido sem sucesso não altera a informação na base de dados.

Para abstrair o código dos *controllers* da criação e obtenção da sessão estes implementam a classe *RavenApiController* e através da propriedade *DbSession* obtêm a instância da Sessão para o pedido. O acesso a esta propriedade tem como resultado a chamada à propriedade *Global.Raven.CurrentSession* que retorna a sessão associada ao pedido ou cria uma nova caso esta não tenha sido criada.

Autenticação 
-

A autenticação na api é feita utilizando HTTP *[Basic authentication](#basic)* ^[http://www.ietf.org/rfc/rfc2617.txt]. 

Quando um pedido passa pelo *pipeline* de processamento de pedidos HTTP é instanciada a classe *BasicAuthenticationHandler* que verifica se o *header* de nome *[Authorization](#http)* ^[http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.8] tem como esquema de autenticação *Basic*.

As credenciais do utilizador são enviadas no pedido em *Base64* e convertidas pelo servidor para validar a autenticação. Depois de convertidas as credenciais tem a forma *nome-de-utilizador:password* e podem ser verificadas utilizando o processo descrito anteriormente (ver Autenticação)

Se o utilizador não preencher o *header* de autenticação e o pedido precisar dessa informação o código da resposta é 401 (*Unauthorized*).

Autorização
-
