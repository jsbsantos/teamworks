Web Api 
=




A Api da infra estrutura é desenvolvida usando modelo de arquitectura ReST[[1]](http://link/) sobre o protocolo HTTP. No modelo ReST todos os objectos da aplicação são considerados recursos e têm um URL único. Todos os acessos à Api, referidos neste documento, têm como base o URL `https://teamworks.apphb.com/api`.

Autenticação
-

A autenticação na Api é feita utilizando HTTP *Basic authentication*. 
É possível testar a autenticação acedendo no browser ao URL:

````
https://nome-de-utilizador:password@teamworks.apphb.com/api/person
````

Formato da data
=

<span style="background-color: yellow">RFC 2822</span>

Códigos de resposta
=

Todas as respostas da Api têm o código mais adequado segundo as normas HTTP para descrever o resultado do pedido.

<span style="background-color: yellow">Tabela com códigos que podem ser retornados pela Api. </span>

Obter projectos
=

Para aceder a todos os projectos disponíveis o URL a aceder é o seguinte:

````
get /projects 
````

<span style="background-color: red; color: white">To be continued...</span>

[1] ReST - Representational state transfer



