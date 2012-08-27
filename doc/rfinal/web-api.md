Web Api 
=

\label{sec:api}

A Api assenta sobre modelo de arquitectura ReSTful e todos os objectos de domínio da aplicação são considerados recursos com URL próprio. 
A comunicação é feita utilizando o protocolo HTTP.

O processamento de um pedido na *framework* ASP.NET Web Api pode ser dividido em três camadas \ref{pfelix}. 

***Hosting***

A primeira, a camada de *hosting*, recebe o pedido, cria uma instância de `HttpRequestMessage` e passa a instância á camada superior. 
Esta camada é também responsável por receber a instância de `HttpResponseMessage` retornada pela camada seguinte. 

***Message handler pipeline***

Depois da camada de *hosting* a instância de `HttpRequestMessage` é passada ao *message handler pipeline*, a segunda camada. 
Todos os *message handler* do *pipeline* derivam da classe `HttpMessageHandler` e o primeiro é do tipo `HttpServer`.
O *pipeline* permite adicionar *message handlers* para processar a mensagem presente no *pipeline* sendo que estes têm de extender `DelegatingHandler` (que deriva de `HttpMessageHandler`). 
A classe `DelegatingHandler` usa o padrão *delegator* [^delegator], sendo que cada instância adicionada passa para a instância seguinte a mensagem criando uma cadeia de processamento sequencial. 
No final do *pipeline* a mensagem é passada a uma instância do tipo `HttpControllerDispatcher` que, utilizando a tabela de *routing*, obtém e invoca o *controller*. 

[^delegator]: O padrão *Delegator* define que cada instância tem a responsabilidade de delegar a execução de uma tarefa para outra instância. 

***Controller***

A última camada é responsável pela invocação do *controller*, nomeadamente:

+ **Seleccionar a *action* a executar**

	A selecção da *action* é baseada no url do pedido e nas *routes* registadas na aplicação. Para o registo de *routes* é usado o projecto *open source* AttributeRouting \ref{attributerouting} que permite definir *routes* através de atributos nos *controllers* e *actions*.

+ **Executar os filtros**
	
	Há a possibilidade de registar filtros globais, associados a um *controller* ou a uma *action*. Os filtros globais são executados para todas as *actions* de todos os *controllers* instanciados. Os filtros associados a um *controller* são executados para todas as *actions*. Os filtros associados a uma *action* são executados para essa *action*.

+ ***Model binding***

+ **Chamar a *action***

+ **Criar a resposta com base nos *formatters* disponíveis**
	
	O formato JSON está configurado para ignorar propriedades a *null*, para usar o formato *CamelCase* no nome dos atributos e para formatar as datas de acordo com o ISO 8601.

O primeiro requisito que tem de ser comtemplado na implementação da Api é a autenticação. Como a autenticação é necessária em todos os pedidos à Api deve ser feita no *pipeline* da *framework* ASP.NET Web Api antes de ser instanciado o *controller* que processa o pedido.

A autenticação é feita pela classe `BasicAuthentication`. No processamento do pedido este *message handler* verifica se o *header* de nome `Authorization` tem como esquema de autenticação *Basic* e converte as credenciais para a forma original, `nome-de-utilizador:password`.
A validação das credenciais enviadas para o servidor utiliza o processo descrito no domínio (ver secção \ref{sec:dominio}).

Na resposta se o código for `401 Unhautorized` este elemento adiciona o *header* de autenticação para indicar que aceita autenticação do tipo *Basic*.

O *message handler* responsável pela autenticação necessita de dados presentes na base de dados, obtidos utilizando uma sessão *Raven*.
A criação da sessão é feita pelo *message handler* `RavenSession` que instância uma sessão de acesso e a adiciona às propriedades do pedido, lista \ref{code:opensession}, disponibilizando a mesma sessão durante o resto do processamento do pedido. 

\lstset{caption={Processamento do pedido da classe `RavenSessionHandler`.},label={code:opensession}}

````
var session = Global.Database.OpenSession();
request.Properties[Application.Keys.RavenDbSessionKey] = session;
````

Se a resposta não contiver erros e decorrer normalmente o *message handler* persiste as alterações à sessão, como demonstra a lista \ref{code:savechanges}. 

\lstset{caption={Processamento da resposta da classe `RavenSessionHandler`.},label={code:savechanges}}

````
using (session) {  
  if (session != null && t.Result.IsSuccessStatusCode) {  
    session.SaveChanges();  
  }  
}  
````

Na figura \ref{fig:russiandoll} estão representados os elementos adicionados ao *pipeline* e a ordem por que são executados, `RavenSession` e `BasicAuthentication`.

![Processamento do pedido na Web Api.\label{fig:russiandoll}](http://www.lucidchart.com/publicSegments/view/50291e63-5070-4845-94a2-5c020a7c36ea/image.png)

*Controllers*
-

Para abstrair as *actions* da obtenção da sessão foi criada a classe `RavenApiController` que disponibiliza a propriedade `DbSession`, que retorna uma sessão para acesso à base de dados.
O propriedade `DbSession` é afectada com a instância passada por parâmetro no construtor do *controller*.

Como indicado anteriormente no fim do *pipeline* é obtido um *controller*. No contexto do controller é seleccionada a *action* que processa o pedido mas antes desta ser chamada são executados os filtros e é feito o *model binding* dos parâmetros da *action*. A *action* é invocada e utilizando os *formatters* registados na *framework* e a negociação de conteúdos com o cliente é criada a resposta.

Os filtros são uma forma de extender o processamento do pedido por parte do *controller* seleccionado.
Os filtros podem ser globais, associados ao *controller* ou a uma *action* e derivam de `ActionFilterAttribute`. 
Os filtros globais são registados no início da aplicação e os restantes como atributos aplicados ao *controller* ou *action* dependendo das necessidades de abrangência do filtro.  

Os atributos definidos na implementação da infra-estrutura são os seguintes:

* `MappingExceptionFilterAttribute`

	O `MappingExceptionFilterAttribute` deriva de `ExceptionFilterAttribute`, um tipo específico de `ActionFilterAttribute`, e para além de redefinir o método `OnException` tem uma propriedade `Mappings` que relaciona um tipo com uma regra(`Rule`). A regra define o código associado e se a mensagem da excepção é incluída no corpo da resposta.

	Como pode ser observado na figura \ref{fig:exceptionfilter} o método redefinido obtém a excepção e dependendo do seu tipo cria a resposta adequada. A resposta criada se a excepção for uma `HttpException` tem o código associado á excepção. Se for de um tipo presente na propriedade `Mappings` a resposta tem o código associado e o corpo definido na regra registada. Por fim se não for do tipo `HttpResponseException` a resposta tem o código `500 Internal Server Error`. O tipo `HttpResponseException` não é considerado porque é processado pela classe base.

![Fluxo de processamento de MappingExceptionFilterAttribute.\label{fig:exceptionfilter}](http://www.lucidchart.com/publicSegments/view/50290444-1734-42a0-844d-48190ad3924f/image.png)

* `ModelStateFilterAttribute`

	Este filtro estende `ActionFilterAttribute` e redefine o método `OnActionExecuting`. Este método é chamado depois de ser feito o *model binding* e antes de ser chamada a *action* para processar o pedido. 
	O filtro verifica o resultado do *model binding* e se contiver erros lança uma excepção HTTP com o código `400 Bad Request` e no corpo com a informação dos campos inválidos. Este comportamento é benéfico pois a verificação de error é feito de forma global e antes de ser chamada a *action*. 

* `SecureForFilterAttribute`

* `RavenSessionFilterAttribute`
	

