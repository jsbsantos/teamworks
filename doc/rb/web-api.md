Web Api 
=

De forma a disponibilizar aos utilizadores uma forma de interagir e integrar aplicações externar com a infra-estrutura foi desenvolvida um serviço web disponibilizado através de uma api web. **(confuso?)**

A web api assenta sobre modelo de arquitectura ReST ^[Representational state transfer] em que todos os objectos da aplicação são considerados recursos e têm URL definido. 
A comunicação é feita utilizando o protocolo HTTP ^[RFC2616 - http://www.w3.org/Protocols/rfc2616/rfc2616.html].

É comum a todos os pedidos à api a necessidade de aceder ao repositório de dados, RavenDB. 
Para isso é usada uma instância da sessão do cliente RavenDB que tem o tempo de vida de um pedido HTTP. 
É criada durante o processamento do pedido e no final as alterações feitas à sessão são persistidas na base de dados pela classe `RavenHandler`. Esta classe adicionada ao pipeline  HTTP verifica se não ocorreu nenhum erro no processamento do pedido e persiste as alterações em caso de sucesso, como demontra o troço seguinte:

````
using (var session = Global.Raven.CurrentSession) {  
    if (session != null && t.Result.IsSuccessStatusCode)  {  
        session.SaveChanges();  
    }  
}  
````

Para abstrair o código dos *controllers* da criação e obtenção da sessão estes implementam a classe `RavenApiController` que implementa a propriedade `DbSession`. A classe `DbSession` retorna a sessão associada ao pedido currente.
O acesso a esta propriedade tem como resultado a chamada à propriedade `Global.Raven.CurrentSession` que retorna a sessão associada ao pedido ou cria uma nova caso esta não tenha sido criada.

Mappers
-

Autenticação 
-

A autenticação na api é feita utilizando HTTP *[Basic authentication](#basic)* 

Quando um pedido passa pelo *pipeline* de processamento de pedidos HTTP é instanciada a classe `BasicAuthenticationHandler` que verifica se o *header* de nome *[Authorization](#http)* ^[http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.8] tem como esquema de autenticação *Basic*.

As credenciais do utilizador são enviadas no pedido em *Base64* e depois de convertidas têm a forma `nome-de-utilizador:password` e podem ser verificadas utilizando o processo descrito anteriormente (ver Autenticação).

Se o utilizador não preencher o *header* de autenticação quando necessário o código da resposta é `401 Unauthorized`.

Autorização
-

<span style="background-color: yellow">SecureFor</span>
