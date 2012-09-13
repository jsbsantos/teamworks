ASP.NET Web Api
=

\label{anexo:aspdotnetwebapi}

O processamento de um pedido na *framework* ASP.NET Web Api é feito em três camadas \ref{pfelix}. 

***Hosting***

A primeira é a camada de *hosting* que recebe o pedido, cria uma instância de `HttpRequestMessage` e passa a instância á camada superior. 
Esta camada é também responsável por receber a instância de `HttpResponseMessage` retornada pela camada seguinte. 

***Message handler pipeline***


A segunda camada é a *message handler pipeline*, que recebe a instância de `HttpRequestMessage` criada na camada de *hosting*. Todos os *message handler* do *pipeline* derivam da classe `HttpMessageHandler`, sendo que o primeiro que é do tipo `HttpServer`.
O *pipeline* permite adicionar *message handlers*, para processar a mensagem presente no *pipeline*, que têm de estender `DelegatingHandler` (que deriva de `HttpMessageHandler`). 
A classe `DelegatingHandler` usa o padrão *delegator* [^delegator], sendo que cada instância adicionada passa para a instância seguinte a mensagem criando uma cadeia de processamento sequencial. 
No final do *pipeline* a mensagem é passada a uma instância do tipo `HttpControllerDispatcher` que, utilizando a tabela de *routing*, obtém e chama o *controller*. 

[^delegator]: O padrão *Delegator* define que cada instância tem a responsabilidade de delegar a execução de uma tarefa para outra instância. 

***Controller***

A última camada está relacionada com o processamento do *controller*, nomeadamente:

+ **Seleccionar a *action* a executar**

	A selecção da *action* é baseada no url do pedido e nas *routes* registadas na aplicação. Para o registo de *routes* é usado o projecto *open source* AttributeRouting \ref{attributerouting} que permite definir *routes* através de atributos nos *controllers* e *actions*.

+ **Executar os filtros**
	
	Há a possibilidade de registar filtros globais, associados a um *controller* ou a uma *action*. Os filtros globais são executados para todas as *actions* de todos os *controllers* instanciados. Os filtros associados a um *controller* são executados para todas as *actions*. Os filtros associados a uma *action* são executados para essa *action*.

+ ***Model binding***

+ **Executar a *action***

+ **Criar a resposta com base nos *formatters* disponíveis**
	
	O formato JSON está configurado para ignorar propriedades a *null*, para usar o formato *CamelCase* no nome dos atributos e para formatar as datas de acordo com o ISO 8601.
