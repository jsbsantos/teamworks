O processamento de um pedido na *framework* ASP.NET Web Api pode ser dividido em três camadas \ref{pfelix}. 

***Hosting***

A primeira, a camada de *hosting*, recebe o pedido, cria uma instancia de `HttpRequestMessage` e passa a instancia á camada superior. 
Esta camada é também responsável por receber a instancia de `HttpResponseMessage` retornada pela camada seguinte. 

***Message handler pipeline***

Depois da camada de *hosting* a instancia de `HttpRequestMessage` é passada ao *message handler pipeline*, a segunda camada. 
Todos os *message handler* do *pipeline* derivam da classe `HttpMessageHandler` e o primeiro é do tipo `HttpServer`.
O *pipeline* permite adicionar *message handlers* para processar a mensagem presente no *pipeline* sendo que estes têm de extender `DelegatingHandler` (que deriva de `HttpMessageHandler`). 
A classe `DelegatingHandler` usa o padrão *delegator* [^delegator], sendo que cada instancia adicionada passa para a instancia seguinte a mensagem criando uma cadeia de processamento sequencial. 
No final do *pipeline* a mensagem é passada a uma instancia do tipo `HttpControllerDispatcher` que, utilizando a tabela de *routing*, obtem e chama o *controller*. 

[^delegator]: O padrão *Delegator* define que cada instancia tem a responsabilidade de delegar a execução de uma tarefa para outra instancia. 

***Controller***

A última camada está relaciona com o processamento do *controller*, nomeadamente:

+ **Selecionar a *action* a executar**

	A seleção da *action* é baseada no url do pedido e nas *routes* registadas na aplicação. Para o registo de *routes* é usado o projecto *open source* AttributeRouting \ref{attributerouting} que permite definir *routes* através de atributos nos *controllers* e *actions*.

+ **Executar os filtros**
	
	À a possibilidade de registar filtros globais, associados a um *controller* ou a uma *action*. Os filtros globais são executados para todas as *actions* de todos os *controllers* instanciados. Os filtros associados a um *controller* são executados para todas as *actions*. Os filtros associados a uma *action* são executados para essa *action*.

+ ***Model binding***

+ **Chamar a *action***

+ **Criar a resposta com base nos *formatters* disponíveis**
	
	O formato JSON está configurado para ignorar propriedades a *null*, para usar o formato *CamelCase* no nome dos atributos e para formatar as datas de acordo com o ISO 8601.
