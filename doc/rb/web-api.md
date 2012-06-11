Web Api 
=

A Web Api assenta sobre modelo de arquitectura ReST ^[Representational state transfer] e a comunicação é feita utilizando o protocolo HTTP. 
No modelo ReST todos os objectos da aplicação são considerados recursos e têm um URL único. 
Como referência para a implementação do protocolo HTTP foi usado o documento RFC2616 ^[http://www.w3.org/Protocols/rfc2616/rfc2616.html].

Unit of Work
-

A Unit of Work tem o tempo de vida de um pedido HTTP, é criada durante o processamento do pedido e no final do pedido as alterações feitas à sessão são persistidas na base de dados.
A persistência é feita depois da *action* correspondente ser executada pela classe *RavenHandler* registada no pipeline HTTP durante o arranque da Api. Esta classe verifica se o pedido foi realizado com sucesso e persiste as alterações usando o código seguinte.

````
using (var session = Global.Raven.CurrentSession) {  
    if (session != null && t.Result.IsSuccessStatusCode)  {  
        session.SaveChanges();  
    }  
}  
````

Caso durante o processamento do pedido ocorra um erro e o código da resposta não for de sucesso as alterações não são persistidas, desta forma um pedido sem sucesso não altera a informação na base de dados.

Para abstrair o código dos *controllers* da criação e obtenção da sessão estes podem implementar a classe *RavenApiController* e através da propriedade *DbSession* obter a sessão currente. O acesso a esta propriedade tem como resultado a chamada à propriedade *Global.Raven.CurrentSession* que retorna a sessão associada ao pedido ou cria uma nova caso esta não tenha sido criada.

Autenticação 
-

A autenticação na Api é feita utilizando HTTP *[Basic authentication](#basic)* ^[http://www.ietf.org/rfc/rfc2617.txt]. 

Quando um pedido passa pelo pipeline de processamento de pedidos HTTP é instânciada a classe *BasicAuthenticationHandler* que verifica se o header de nome *[Authorization](#http)* ^[http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.8] tem como esquema de autenticação *Basic*.

As credencias do utilizador são enviadas no pedido em Base64 e convertidas pelo servidor para validar a autenticação. Depois de convertidas as credenciais tem a forma nome-de-utilizador:password e podem ser verificadas utilizando o processo descrito anteriormente (ver Autenticação)

Se o utilizador não preencher o Header de autenticação e o pedido precisar dessa informação o código da resposta é 401 (Unauthorized).

Autorização
-
