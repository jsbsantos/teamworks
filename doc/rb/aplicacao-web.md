Aplicação Web
=

\label{sec:app-web}

A aplicação web disponibiliza uma interface para interacção com a infra-estrutura.
Para a implementar é usada a *framework* [ASP.NET MVC](#aspnetmvc),que implementa o padrão MVC^[MVC - **M**odel–**v**iew–**c**ontroller].

A aplicação web disponibiliza *controllers* que têm como resposta uma página web com código *javascript* para acesso à web api, tornando o browser um cliente da api. 
As páginas web retornadas utilizam as *frameworks javascript* jQuery[#jquery]() e knockout[#knockout]() para interacção com o utilizador e comunicação com a api.

A componente visual é conseguida usando HTML5, CSS3 e o *kit* Twitter Bootstrap[#bootstrap]().

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação dos dados inseridos é feita de forma semelhante à utilizada na web api com a diferença que na aplicação web é usada uma cookie (*.tw_auth*) para manter o utilizador autenticado nos pedidos subsequentes. 
Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* ASP.NET[#aspnet](). 

As classes que expõem as funcionalidades necessárias são a classe `FormsAuthentication`, usada para gerir o cookie de autenticação^[A cookie usada tem o nome *.tw_auth*], e o módulo `FormsAuthenticationModule` para manter o utilizador autenticado. 

Depois de validar os dados do utilizador com sucesso a cookie é colocada na resposta usando a classe `FormsAuthentication`.
A cookie tem como valor o identificador da instância de *Person* que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo `FormsAuthenticationModule`, em cada pedido, coloca o valor da *cookie* na propriedade `User` do pedido. A acção do módulo é complementada pelo filtro `FormsAuthenticationAttribute` que substitui o `IIdentity` do pedido por um `PersonIdentity` que disponibiliza a entidade `Person` do utilizador autenticado ao código executado nas *actions*.

Para que um utilizador quando autenticado na aplicação web também esteja autenticado na api foi acrescentado ao *pipeline* da api a classe `FormsAuthenticationHandler`.
Esta implementação de `DelegatingHandler` verifica se o `IPricipal` do pedido está autenticado e se contiver uma instância de `PersonIdentity` coloca a `Person` correspondente na colecção `Properties` utilizando a chave `HttpPropertyKeys.UserPrincipalKey`.

Na aplicação web um utilizador está autorizado a aceder a qualquer um dos *controllers* que interagem com a api desde que autenticado. 

Cliente da api
-

