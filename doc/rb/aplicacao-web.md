Aplicação Web
=

Para disponibilizar ao utilizador uma interface para interagir com a infra-estrutura através de qualquer *browser*, foi criada uma aplicação Web. Para a sua implementação é usada a *framework* [ASP.NET MVC](#aspnetmvc). Esta framework implementa o padrão MVC^[Model–view–controller].

A componente visual da aplicação web é conseguida usando HTML5 e CSS3 e o aspecto utilizando os componentes disponibilizados no *kit* [Twitter Bootstrap](#bootstrap). 

Para a interacção do utilizador com a página Web e comunicação com a Web Api é usada a *framework javascript* [knockout](#knockout).

<span style="background-color: yellow">Falar da utilização de knockout</span>

Para facilitar o acesso á Api por parte da aplicação Web foi acrescentado ao pipeline de processamento dos pedidos da Api a classe *FormsAuthenticationHandler*.
Esta implementação de *DelegatingHandler* verifica se o *Pricipal* do pedido está autenticado e se contiver uma *Identity* do tipo *PersonIdentity* coloca a Person correspondente no pedido utilizando a chave HttpPropertyKeys.UserPrincipalKey na colecção de *Properties*.

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação é semelhante à utilizada na web api com a diferença que na aplicação web é usada uma cookie (*.tw_auth*) para manter o utilizador autenticado nos pedidos subsequentes. 
Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* [ASP.NET](#aspnet). 

As classes que expõe as funcionalidades necessárias são a classe *FormsAuthentication*, usada para gerir o cookie de autenticação^[A cookie usada tem o nome *.tw_auth*], e o módulo HTTP *FormsAuthenticationModule* para manter o utilizador autenticado. 

Os dados inseridos pelo utilizador são validados e em caso de sucesso a cookie é colocada na resposta usando a classe *FormsAuthentication*.
Como valor a cookie tem o identificador da Person que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo *FormsAuthenticationModule* em cada pedido coloca o valor da cookie na propriedade *User* do pedido. A acção do módulo é complementada pela filtro *FormsAuthenticationAttribute* que substitui o *IIdentity* do pedido por um *PersonIdentity* que disponibiliza a entidade Person do utilizador autenticado.

