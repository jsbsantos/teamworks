Aplicação Web
=

Para disponibilizar ao utilizador uma interface para aceder aos dados da infra-estrutura através de qualquer *browser*, existe uma aplicação Web. Para a implementação deste componente é usada a *framework ASP.NET MVC* (aspnetmvc)<!---cite-->, leccionada no decorrer do curso. Esta framework, como o próprio nome sugere, implementa o padrão *model-view-controller* (MVC).

Na implementação da componente visual da aplicação web é usado HTML5 e CSS3 e o aspecto visual é conseguido utilizando os componentes disponibilizados no *kit Twitter Bootstrap* (bootstrap)<!---cite-->. As *frameworks javascript jQuery* (jquery)<!---cite--> e *Knockout* (knockout)<!---cite--> permitem enriquecer a forma como é feita a interação com a aplicação.

Autenticação
-

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. 
Para manter o utilizador autenticado são usadas funcionalidades do modo de autenticação *forms* da *framework* [ASP.NET](http://asp.net)<!---dump--> com o modo *forms*. As classes que expõe as funcionalidades são a classe *FormsAuthentication*, usada para gerir o cookie de autenticação (*.tw_auth*), e o módulo HTTP *FormsAuthenticationModule*, que coloca o valor da cookie na informação de cada pedido (*HttpContext.User.Identity.Name*). 

Utilizando a classe FormsAuthentication o cookie *.tw_auth* é colocado na resposta com o identificador da Person autenticada. 

Para disponibilizar a entidade *Person* autenticada nas *action* chamadas foi definido atributo *FormsAuthenticationAttribute* que substitui o *IIdentity* do pedido por um *PersonIdentity* que internamente guarda uma instância de Person. Para que esta alteração não afete o comportamento da autenticação por *forms* no final do pedido o *IIdentity* é novamente alterado para ter como *Name* o identificador da *Person* autenticada.

Unit of Work
-

**Falar da autenticação através da webapi**

Funcionalidades
-

Configuração
-
