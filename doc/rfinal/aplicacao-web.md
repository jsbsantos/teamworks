Aplicação Web
=

\label{sec:app-web}

A aplicação web disponibiliza uma interface que possibilita que o utilizador interaja com a infraestrutura, implementada usando a *framework* ASP.NET MVC \cite{aspnetmvc}.
Numa primeira fase foi decidido que a aplicação Web era apenas um consumidor da Api obtendo toda a informação que necessitava apresentar ao utilizador da Api.
Com o decorrer do projecto conclui-se que os dados relevantes a apresentar pela aplicação Web não eram possíveis obter da web Api sem desvirtuar o objectivo da Api de expor os objectos de domínio como recursos.

A componente visual é conseguida usando HTML5, CSS3 e o *kit* Twitter Bootstrap \cite{bootstrap}.

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação dos dados inseridos é feita de forma semelhante à utilizada na Api, com a diferença que na aplicação web é usada uma cookie para manter o utilizador autenticado nos pedidos subsequentes. Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* ASP.NET. 


As classes que expõem as funcionalidades necessárias são a classe `FormsAuthentication`, usada para gerir o cookie de autenticação[^cookie], e o módulo `FormsAuthenticationModule` para manter o utilizador autenticado. 

Além da autenticação por utilizador e password, é possivel que o utilizador faça o registo e autenticação através da utilização de um provider OpenID \cite{OpenID}., sendo que apenas é suportado o provider OpenID da Google.

[^cookie]: A cookie usada tem o nome *.tw_auth*

Depois de validar os dados do utilizador com sucesso a cookie é colocada na resposta usando a classe `FormsAuthentication`.
A cookie tem como valor o identificador da instância de *Person* que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo `FormsAuthenticationModule`, em cada pedido, coloca o valor da *cookie* na propriedade `User` do pedido. A acção do módulo é complementada pelo filtro `FormsAuthenticationAttribute` que substitui o `IIdentity` do pedido por um `PersonIdentity` que disponibiliza a entidade `Person` do utilizador autenticado ao código executado nas *actions*.

Como forma de tornar as páginas web mais dinâmicas e melhorando a experiência do utilizador, foi usada a frameworks javascript Knockout. Esta framework implementa o padrão MVVM (**M**odel-**V**iew-**V**iew**M**odel) e faz a actualização dos elementos da página, de acordo com as alterações ao modelo (viewmodel) que lhe está associado, feitas através de acções do utilizador.

Este comportamento é conseguido definindo, em javascript, um *View Model* que representa a informação apresentada na página. Estes View Models definem propriedades observáveis (observables) por elementos HTML, através de javascript. Através desta ligação é possivel o valor do elemento HTML seja alterado consoante o valor do atributo que observa e vice versa.
 
***ActionFilterAttribute***

AjaxOnlyAttribute
FormsAuthenticationAttribute	

SecureAttribute
SecureProjectAttribute

É possível fazer um paralelismo entre a implementação da aplicação Web e da Api. 

