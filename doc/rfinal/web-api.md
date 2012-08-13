Web Api 
=

\label{sec:api}

A Api assenta sobre modelo de arquitectura ReSTful e todos os objectos de domínio da aplicação são considerados recursos com URL próprio. 
A comunicação é feita utilizando o protocolo HTTP.

O processamento do pedido HTTP é iniciado no *pipeline* da *framework* ASP.NET Web Api.
O primeiro elemento do *pipeline* é do tipo `HttpServer` e o último elemento do tipo `HttpControllerDispatcher` e tem como responsabilidade, utilizando a tabela de *routing*, obter e chamar o *controller* responsáavel por processar o pedido. Todos os elementos do *pipeline* derivam da classe `HttpMessageHandler`.

É possível extender o *pipeline* adicionando novos elementos antes do processamento do *controller* tendo estes que derivar de `DelegatingHandler`(que deriva de `HttpMessageHandler`). A classe `DelegatingHandler` usa o padrão *delegator* que define que cada instancia tem de referenciar uma instancia de `DelegatingHandler`. Desta forma cria-se uma cadeia de elementos por onde passa o pedido de forma sequencial por todos os elementos do *pipeline*.

Na figura \ref{fig:russiandoll} estão representados os elementos adicionados ao *pipeline* e a ordem por que são executados, `RavenSession` e `BasicAuthentication`.

![Processamento do pedido na Web Api.\label{fig:russiandoll}](http://www.lucidchart.com/publicSegments/view/50291e63-5070-4845-94a2-5c020a7c36ea/image.png)

* `RavenSession`

	O elemento `RavenSession` instancia uma sessão de acesso à base de dados e adiciona-a às propriedades do pedido, lista \ref{code:opensession}. 

	Na resposta este elemento verifica se não ocorreu nenhum erro no processamento do pedido, persistindo as alterações em caso de sucesso, como demonstra a lista \ref{code:savechanges}. 

\lstset{caption={Processamento do pedido da classe `RavenSessionHandler`.},label={code:opensession}}

````
var session = Global.Database.OpenSession();
request.Properties[Application.Keys.RavenDbSessionKey] = session;
````

\lstset{caption={Processamento da resposta da classe `RavenSessionHandler`.},label={code:savechanges}}

````
using (session) {  
  if (session != null && t.Result.IsSuccessStatusCode) {  
    session.SaveChanges();  
  }  
}  
````

* `BasicAuthentication`

	No processamento do pedido o elemento `BasicAuthentication` verifica se o *header* de nome `Authorization` tem como esquema de autenticação *Basic* e converte as credenciais para a forma original, `nome-de-utilizador:password`.
	A validação das credenciais enviadas para o servidor utiliza o processo descrito no domínio (ver secção \ref{sec:dominio}).

	Na resposta se o código for `401 Unhautorized` este elemento adiciona o *header* de autenticação para indicar que aceita autenticação do tipo *Basic*.

*Controllers*
-

Para abstrair as *actions* da obtenção da sessão foi criada a classe `RavenApiController` que disponibiliza a propriedade `DbSession`, que retorna uma sessão para acesso à base de dados.
O propriedade `DbSession` é afectada com a instancia passada por parâmetro no construtor do *controller*.

Como indicado anteriormente no fim do *pipeline* é obtido um *controller*. No contexto do controller é seleccionada a *action* que processa o pedido mas antes desta ser chamada são executados os filtros e é feito o *model binding* dos parametros da *action*. A *action* é invocada e utilizando os *formatters* registados na *framework* e a negociação de conteúdos com o cliente é criada a resposta.

Os filtros são uma forma de extender o processamento do pedido por parte do *controller* selecionado.
Os filtros podem ser globais, associados ao *controller* ou a uma *action* e derivam de `ActionFilterAttribute`. 
Os filtros globais são registados no inicio da aplicação e os restantes como atributos aplicados ao *controller* ou *action* dependendo das necessidades de abrangência do filtro.  

Os atributos definidos na implementação da infra-estrutura são os seguintes:

* `MappingExceptionFilterAttribute`

	O `MappingExceptionFilterAttribute` deriva de `ExceptionFilterAttribute`, um tipo especifico de `ActionFilterAttribute`, e para além de redefinir o método `OnException` tem uma propriedade `Mappings` que relaciona um tipo com uma regra(`Rule`). A regra define o código associado e se a mensagem da excepção é incluída no corpo da resposta.

	Como pode ser observado na figura \ref{fig:exceptionfilter} o método redefinido obtem a excepção e dependendo do seu tipo cria a resposta adequada. A resposta criada se a excepção for uma `HttpException` tem o código associado á excepção. Se for de um tipo presente na propriedade `Mappings` a resposta tem o código associado e o corpo definido na regra registada. Por fim se não for do tipo `HttpResponseException` a resposta tem o código `500 Internal Server Error`. O tipo `HttpResponseException` não é considerado porque é processado pela classe base.

![Fluxo de processamento de MappingExceptionFilterAttribute.\label{fig:exceptionfilter}](http://www.lucidchart.com/publicSegments/view/50290444-1734-42a0-844d-48190ad3924f/image.png)

* `ModelStateFilterAttribute`

	Este filtro extende `ActionFilterAttribute` e redefine o método `OnActionExecuting`. Este método é chamado depois de ser feito o *model binding* e antes de ser chamada a *action* para processar o pedido. 
	O filtro verifica o resultado do *model binding* e se contiver erros lança uma excepção HTTP com o código `400 Bad Request` e no corpo com a informação dos campos inválidos. Este comportamento é benefico pois a verificação de error é feito de forma global e antes de ser chamada a *action*. 

* `SecureForFilterAttribute`

* `RavenSessionFilterAttribute`
	

