ASP.NET Web Api
=

\label{anexo:aspdotnetwebapi}

ASP.NET Web API é uma framework que usada para construir serviços HTTP usando o modelo de arquitectura ReSTful, tornando-os acessíveis através de uma vasta gama de clientes, browsers e dispositivos. 
O processamento de um pedido na *framework* ASP.NET Web Api é feito em três camadas: *hosting*, *pipeline* e o processamento do *controller* \cite{pfelix}.

***Hosting***

A primeira camada, *hosting*, recebe o pedido, cria uma instância de `HttpServer` e passa a á camada superior.
Esta camada é também responsável por receber a instância de `HttpResponseMessage` retornada pela camada seguinte. 

***Pipeline***

A camada *pipepline* é composta por instâncias de `HttpMessageHandler` sendo encabeçado pela instância de `HttpServer` criada na camada de *hosting*.
O *pipeline* permite adicionar *message handlers*, para processar a mensagem presente no *pipeline*, que estendem `DelegatingHandler`. 
A classe `DelegatingHandler` usa o padrão *delegator* [^delegator], sendo que cada instância adicionada passa para a instância seguinte a mensagem, criando uma cadeia de processamento sequencial. 
No final do *pipeline* a mensagem é passada a uma instância do tipo `HttpControllerDispatcher` que, utilizando a tabela de *routing*, obtém e chama o *controller*. 

[^delegator]: O padrão *Delegator* define que cada instância tem a responsabilidade de delegar a execução de uma tarefa para outra instância. 

**Processamento do *Controller***

A última camada está relacionada com o processamento do *controller*. As suas funções são: seleccionar a *action*, executar os filtros, proceder ao *model binding*, executar a *action*, criar a resposta com base nos *formatters* disponíveis.

A selecção da *action* é baseada no url do pedido e nas *routes* registadas na aplicação.
Há a possibilidade de registar filtros globais, associados a um *controller* ou a uma *action*. Os filtros globais são executados para todas as *actions* de todos os *controllers* instanciados. Os filtros associados a um *controller* são executados para todas as *actions*. Os filtros associados a uma *action* são executados para essa *action*.
