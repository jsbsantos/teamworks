Web Api 
=

\label{sec:api}

A Api assenta sobre modelo de arquitectura ReSTful e utiliza a comunicação é feita usando o protocolo HTTP.
Todos os objectos de domínio da aplicação são considerados recursos com URL próprio e o processamento dos pedidos é feito utilizando a *framework* Asp.Net Web Api \ref{anexo:aspdotnetwebapi}.
Para implementação da solução foram criados novos *message handlers*, *filters* e *controllers*.

***Message handlers***

Numa aplicação Web todo o processamento de um pedido pode estar relacionado, assim todas as interações com a base de dados devem ser feitas utilizando a mesma sessão (utilização do padrão *unit of work*).
A criação da sessão é feita pelo *message handler* `RavenSession` que instancia uma sessão e a adiciona às propriedades do pedido, lista \ref{code:opensession}, disponibilizando a sessão para que possa ser usada durante o processamento do pedido. 

\lstset{caption={Processamento do pedido da classe `RavenSession`.},label={code:opensession}}

````
var session = Global.Database.OpenSession();
request.Properties[Application.Keys.RavenDbSessionKey] = session;
````

Na resposta ao pedido a instancia de `RavenSession` se a resposta não contiver um  código de erro completa o padrão *unit of work* persistindo as alterações à sessão na base de dados, como demonstra a lista \ref{code:savechanges}. 

\lstset{caption={Processamento da resposta da classe `RavenSessionHandler`.},label={code:savechanges}}

````
using (session) {  
  if (session != null && t.Result.IsSuccessStatusCode) {  
    session.SaveChanges();  
  }  
}  
````

Todos os pedidos à Api têm de ser autenticados e a classe `BasicAuthentication` (derivada de `DelegatingHandler`) é responsável pela autenticação.

Para autenticar os utilizadores é verificada a presença do *header* de nome `Authorization` e se o seu valor é `Basic`. As credenciais são convertidas para a forma original, `nome-de-utilizador:password` e validadas segundo o processo descrito no domínio (ver secção \ref{sec:dominio}).

À semelhança do *message handler* de sessão este também tem responsabilidades na resposta ao pedido. Caso a autenticação não seja válida este *message handler* é, também, responsável por colocar o *header* de autenticação na resposta. O valor do *header* é `Basic` e só é colocado se a resposta tiver como código `401 Unhauthorized`.
Para a autenticação são necessários dados da base de dados. Dados que podem ser obtidos com uma sessão Raven \ref{a}.

Na figura \ref{fig:russiandoll} estão representados os elementos adicionados ao *pipeline* e a ordem por que são executados, `RavenSession` e `BasicAuthentication`.

![Processamento do pedido na Web Api.\label{fig:russiandoll}](http://www.lucidchart.com/publicSegments/view/50291e63-5070-4845-94a2-5c020a7c36ea/image.png)


***Filters***

para a implementação da Api foram criados os seguintes filtros: `MappingExceptionFilterAttribute`, `ModelStateFilterAttribute`, `SecureForFilterAttribute` e `RavenSessionFilterAttribute`

O `ExceptionAttribute` deriva de `ExceptionFilterAttribute `, um tipo especifico de `ActionFilterAttribute`, e para além de redefinir o método `OnException` tem uma propriedade `Mappings` que relaciona um tipo com uma regra(`Rule`). A regra define o código associado à excepção e se no corpo da resposta é incluida a mensagem da excepção.

Como pode ser observado na figura \ref{fig:exceptionfilter} o método redefinido obtem a excepção e cria a resposta com base no tipo da excepção. 
Uma excepção do tipo `HttpException` resulta numa resposta com o código e a mensagem associada à excepção. 
De seguida é verificado se a excepção está registada na propriedade `Mappings`. Se for de um tipo presente na propriedade `Mappings` a resposta tem o código definido na regra. Por fim se não for do tipo `HttpResponseException` a resposta tem o código `500 Internal Server Error`. 

![Fluxo de processamento de `ExceptionAttribute`.\label{fig:exceptionfilter}](http://www.lucidchart.com/publicSegments/view/50290444-1734-42a0-844d-48190ad3924f/image.png)

As excepções processadas por este atributo são do tipo `ReadVetoException` e `ArgumentException`. 
A excepção `ReadVetoException` é lançada quando o utilizador não tem acesso ao recurso o que origina o código `404 Not Found`. A excepção `ArgumentException` resulta numa resposta com o código `400 BadRequest`.

O filtro `ModelStateFilterAttribute` extende `ActionFilterAttribute` e redefine o método `OnActionExecuting`. Este método é chamado depois de ser feito o *model binding* e antes de ser chamada a *action* para processar o pedido. 
O filtro verifica o resultado do *model binding* e se contiver erros lança uma excepção HTTP com o código `400 Bad Request` no corpo com a informação dos campos inválidos. Este comportamento tem a vantagem de verificar erros de forma global e antes de ser chamada a *action*. 

#todo

O filtro `RavenSessionFilterAttribute` funciona em conjunto com o *message handler* apresentado anteriormente, implementado na classe `RavenSession`. 
Este filtro redefine o método exectutado antes se ser chamada a *action* e injecta no *controller* a sessão `Raven` presente nas propriedades do pedido. 
A propriedade afectada com o valor da sessão é qualquer uma que tenha como valor de retorno `IDocumentSession`.

*Controllers*
-

Para abstrair as *actions* da obtenção da sessão foi criada a classe `RavenApiController` que disponibiliza a propriedade `DbSession`, que retorna uma sessão para acesso à base de dados.

----


