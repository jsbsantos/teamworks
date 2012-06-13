Aplicação Web
=

A aplicação web disponibiliza uma interface para interacção com a infra-estrutura.
Para a  implementar é usada a *framework* [ASP.NET MVC](#aspnetmvc).
Esta framework implementa o padrão MVC^[Model–view–controller].

A componente visual da aplicação web é conseguida usando HTML5, CSS3 e o *kit* [Twitter Bootstrap](#bootstrap), do qual são usados componentes pré-definidos. 

Para que exista dinamismo e resposta imediata na interacção do utilizador com as páginas web, através da comunicação com a web api e actualização assíncrona da página, é usada a *framework javascript* [knockout](#knockout).

Knockout
-

Autenticação
-

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação é semelhante à utilizada na web api com a diferença que na aplicação web é usada uma cookie (*.tw_auth*) para manter o utilizador autenticado nos pedidos subsequentes. 
Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* [ASP.NET](#aspnet). 

As classes que expõem as funcionalidades necessárias são a classe `FormsAuthentication`, usada para gerir o cookie de autenticação^[A cookie usada tem o nome *.tw_auth*], e o módulo HTTP `FormsAuthenticationModule` para manter o utilizador autenticado. 

Os dados inseridos pelo utilizador são validados e em caso de sucesso a cookie é colocada na resposta usando a classe `FormsAuthentication`.
A cookie tem como valor o identificador da instância de Person que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo `FormsAuthenticationModule` em cada pedido coloca o valor da cookie na propriedade *User* do pedido. A acção do módulo é complementada pelo filtro `FormsAuthenticationAttribute` que substitui o `IIdentity` do pedido por um `PersonIdentity` que disponibiliza a entidade `Person` do utilizador autenticado.

Para facilitar o acesso á api, por intermédio da aplicação web, foi acrescentado ao *pipeline* de processamento dos pedidos da api a classe `FormsAuthenticationHandler`. Esta implementação de `DelegatingHandler` verifica se o `IPricipal` do pedido está autenticado e se contiver uma instância de `PersonIdentity` coloca a `Person` correspondente no pedido utilizando a chave `HttpPropertyKeys.UserPrincipalKey` na colecção `Properties`.

Autorização
-

