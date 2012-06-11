Web Api 
=

A Web Api assenta sobre modelo de arquitectura ReST ^[Representational state transfer] e a comunicação é feita utilizando o protocolo HTTP. 
No modelo ReST todos os objectos da aplicação são considerados recursos e têm um URL único. 
Como referência para a implementação do protocolo HTTP foi usado o documento RFC2616 ^[http://www.w3.org/Protocols/rfc2616/rfc2616.html].

<span style="background-color: yellow">RFC 2822</span>

Autenticação 
-

A autenticação na Api é feita utilizando HTTP *Basic authentication*^[http://www.ietf.org/rfc/rfc2617.txt]. 

Quando um pedido passa pelo pipeline de processamento de pedidos HTTP é instânciada a classe *BasicAuthenticationHandler* que verifica se o header de nome *Authorization* ^[http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.8] tem como esquema de autenticação *Basic*.

As credencias do utilizador são enviadas no pedido em base 64 sendo necessário converter de base 64 para puder validar a autenticação. Depois de convertidas as credenciais tem a forma nome-de-utilizador:password e podem ser verificadas utilizando o processo descrito anteriormente (ver Autenticação)

É possível testar a autenticação acedendo no browser ao URL:
````
https://johndoe:password@teamworks.apphb.com/api/person
````

Se o utilizador não preencher o Header de autenticação e for necessário. A código da resposta é 401 Unauthorized.

Unit of Work
-

Todos os *controllers* da aplicação web são uma implementação do *controller* RavenApiController que tem a propriedade *DbSession* criada para abstrair a obtensão da sessão e para tornar o código mais legível. 
O acesso a esta propriedade tem como resultado a chamada à propriedade Global.Raven.CurrentSession.

No final de cada pedido as alterações feitas à sessão tem de ser persistidas na base de dados para isso ao pipeline HTTP foi adicionado a classe *RavenHandler*. Esta classe no final de cada pedido verifica se o pedido foi realizado com sucesso e persiste as alterações feitas à sessão, como se pode ver no código seguinte.

````
using (var session = Global.Raven.CurrentSession) {  
    if (session != null && t.Result.IsSuccessStatusCode)  {  
        session.SaveChanges();  
    }  
}  
````

Endereços Disponiveis na Api
-

Todos os acessos à Api, referidos neste documento, têm como base o URL `https://teamworks.apphb.com/api`.

<span style="background-color: red; color: white">Completar</span>