Aplicação Web
=

Para disponibilizar ao utilizador uma interface para aceder aos dados da infra-estrutura através de qualquer *browser*, foi criada uma aplicação Web. Para a implementação deste componente é usada a *framework ASP.NET MVC* (aspnetmvc)<!---cite-->, leccionada no decorrer do curso. Esta framework implementa o padrão *model-view-controller* (MVC).

Na implementação da componente visual da aplicação web é usado HTML5 e CSS3 e o aspecto visual é conseguido utilizando os componentes disponibilizados no *kit Twitter Bootstrap* (bootstrap)<!---cite-->. As *frameworks javascript jQuery* (jquery)<!---cite--> e *Knockout* (knockout)<!---cite--> permitem enriquecer a forma como é feita a interação com a aplicação.

Autenticação
-

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação é semelhante à utilizada na web api com a diferença que na aplicação web é usada uma cookie (*.tw_auth*) para manter o utilizador autenticado nos pedidos subsequentes. 
Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* [ASP.NET](http://asp.net)<!---dump-->. 

As classes que expõe as funcionalidades necessárias são a classe *FormsAuthentication*, usada para gerir o cookie de autenticação (*.tw_auth*), e o módulo HTTP *FormsAuthenticationModule* para manter o utilizador autenticado. 
Quando os dados indicados pelo utilizador são válidos é utilizada a classe FormsAuthentication para colocar o cookie *.tw_auth* na resposta com o identificador da Person corresponte ao utilizador autenticado. 
Em cada pedido o módulo *FormsAuthenticationModule* coloca o valor presente na cookie na propriedade *User* do pedido.

Para as actions chamadas terem acesso à entidade *Person* do utilizador autenticado foi definido atributo *FormsAuthenticationAttribute* que substitui o *IIdentity* do pedido por um *PersonIdentity* que internamente guarda a instância de Person.

Para facilitar o acesso á Web Api por parte da aplicação Web foi acrescentado ao pipeline de processamento dos pedidos da Api a classe *FormsAuthenticationHandler*.
Esta implementação de *DelegatingHandler* verifica se a propriedade User do pedido foi alterada para um utilizador autenticado e coloca a Person correspondente nas propriedades do pedido utilizando a chave HttpPropertyKeys.UserPrincipalKey.