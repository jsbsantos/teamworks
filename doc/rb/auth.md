Registo
=

Para utilizar a aplicação é necessário que o utilizador se registe. No registo o utilizador indica o email, usado pela aplicação como forma de comunicar; o nome de utilizador, para o identificar; e a password que em conjunto com o nome de utilizador é usada para autenticar o utilizador.

Na base de dados é guardado o *hash* da *password*. O *hash* é gerado usando um algoritmo de dispersão (SHA-256) que tem como entrada a concatenação da password com um salto aleatório.

Autenticação
=

A única forma de autenticação na solução é usando o nome de utilizador e a *password*. O nome de utilizador é usado para obter a entidade Person (people/nome-de-utilizador), que representa o utilizador, e o *hash* da *password* comparado com o presente na Person obtida. O *hash* é obtido usando a mesma função de dispersão e o mesmo salto utilizado no registo do utilizador.

### Aplicação web

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. 

Para manter o utilizador autenticado são usadas funcionalidades do modo de autenticação *forms* da *framework* [ASP.NET](http://asp.net)<!--- --> com o modo **. As classes que expõe as funcionalidades são a classe *FormsAuthentication*, usada para gerir o cookie de autenticação (*.tw_auth*), e o módulo HTTP *FormsAuthenticationModule*, que coloca o valor da cookie na informação de cada pedido (*HttpContext.User.Identity.Name*). 

Utilizando a classe FormsAuthentication o cookie *.tw_auth* é colocado na resposta com o identificador da Person autenticada. 

Para disponibilizar a entidade *Person* autenticada nas *action* chamadas foi definido atributo *FormsAuthenticationAttribute* que substitui o *IIdentity* do pedido por um *PersonIdentity* que internamente guarda uma instância de Person. Para que esta alteração não afete o comportamento da autenticação por *forms* no final do pedido o *IIdentity* é novamente alterado para ter como *Name* o identificador da *Person* autenticada.

### Api

A autenticação da Api é 

---

