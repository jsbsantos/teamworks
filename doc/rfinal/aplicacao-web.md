Aplicação Web
=

\label{sec:app-web}

A aplicação web disponibiliza uma interface para interacção com a infra-estrutura, implementada usando a *framework* ASP.NET MVC \cite{aspnetmvc}.
Numa primeira fase foi decidido que a aplicação Web era apenas um consumidor da Api obtendo toda a informação que necessitava apresentar ao utilizador da Api.
Com o decorrer do projecto conclui-se que os dados relevantes a apresentar pela aplicação Web não eram possíveis obter da web Api sem desvirtuar o objectivo da Api de expor os objectos de domínio como recursos.

A componente visual é conseguida usando HTML5, CSS3 e o *kit* Twitter Bootstrap \cite{bootstrap}.

A autenticação na aplicação web é feita através de um formulário onde o utilizador insere o nome de utilizador e a *password*. A validação dos dados inseridos é feita de forma semelhante à utilizada na Api com a diferença que na aplicação web é usada uma cookie para manter o utilizador autenticado nos pedidos subsequentes. 
Para isso são usadas funcionalidades do modo de autenticação *forms* da *framework* ASP.NET. 

As classes que expõem as funcionalidades necessárias são a classe `FormsAuthentication`, usada para gerir o cookie de autenticação[^cookie], e o módulo `FormsAuthenticationModule` para manter o utilizador autenticado. 

[^cookie]: A cookie usada tem o nome *.tw_auth*

Depois de validar os dados do utilizador com sucesso a cookie é colocada na resposta usando a classe `FormsAuthentication`.
A cookie tem como valor o identificador da instância de *Person* que foi obtida da base de dados no processo de autenticação. 

O utilizador mantém a identidade dos pedidos anteriores pois o módulo `FormsAuthenticationModule`, em cada pedido, coloca o valor da *cookie* na propriedade `User` do pedido. A acção do módulo é complementada pelo filtro `FormsAuthenticationAttribute` que substitui o `IIdentity` do pedido por um `PersonIdentity` que disponibiliza a entidade `Person` do utilizador autenticado ao código executado nas *actions*.

Na aplicação web um utilizador está autorizado a aceder a qualquer um dos *controllers* que interagem com a Api desde que autenticado. 


A forma como foi desenvolvida a aplicação web torna-a um cliente da Api da plataforma. Este comportamento é conseguido através da utilização da framework javascript knockout e AJAX. Esta framework é usada para fazer actualização de elementos da página web com a informação do servidor, respondendo a interacções com o utilizador. Neste componente é usado o padrão MVVM (**M**odel-**V**iew-**V**iew**M**odel) para tornar as páginas web mais dinâmicas e melhorando a experiência do utilizador. 

Este comportamento é conseguido definindo, em javascript, um *View Model* que representa do dados retornados pela Api. Estes View Models definem propriedades observáveis por elementos HTML, através de javascript. Através desta ligação é possivel o valor do elemento HTML seja alterado consoante o valor do atributo que observa e vice versa.
 
