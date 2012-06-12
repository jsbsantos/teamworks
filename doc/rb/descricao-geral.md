Descrição Geral
=

A solução é composta por recursos criados e geridos por utilizadores.
Para acesso e gestão dos recursos o utilizador precisa de estar autenticado. 
Como se pode ver na figura [usecaseanonimo](#) a única acção possível a um utilizador anónimo é o registo e a autenticação.

![Caso de utilização de utilizador não autenticado.\label{usecaseanonimo}](http://www.lucidchart.com/publicSegments/view/4fd71023-3b68-497b-b199-60a50a443549/image.png)

Depois de autenticados as acções que os utilizadores podem fazer dependem das permissões dos recursos acedidos. Os recursos acedidos pelo utilizador podem ser projectos, tarefas, registo de horas, discussões.

![Caso de utilização de um utilizador autenticado sobre o recurso projecto.\label{usecaseautenticadoprojecto}](http://www.lucidchart.com/publicSegments/view/4fd713d7-a21c-42a8-a3f5-6a740adcb320/image.png)

Arquitectura
-

A solução está dividida em três grandes blocos, o cliente, o servidor e os serviços externos.

![Arquitetura da infra-estrutura implementada\label{architecture}](https://dl.dropbox.com/s/mr7yybyzbm6umu3/architecture.png)

A implementação da Api utiliza a framework ASP.NET Web Api. 