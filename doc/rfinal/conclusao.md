Conclusão
=

A solução desenvolvida atinge os objectivos propostos permitindo aos utilizadores fazer diversas acções sobre a infra-estrutura, como a criação e gestão de projectos, tarefas e debates. 

No desenvolvimento deste projecto foram usadas técnicas e tecnologias que não fazem parte do actual plano de estudos do curso de Licenciatura em Engenharia Informática de Computadores, com as quais não estávamos familiarizados, e que requereram um período de aprendizagem. Sendo este um projecto académico sentimos que era uma boa oportunidade para aprender e utilizar estas novas tecnologias. Entendemos que a aprendizagem e utilização de novas técnicas e tecnologias é vantajosa para o desenvolvimento das nossas competências e para alargar o nosso espectro de conhecimento. 

São exemplos dessas tecnologias a base de dados de documentos *RavenDB*, a framework javascript *knockout* e a framework ASP.NET MVC 4 Web Api. A necessidade de aprofundar o conhecimento nestas tecnologias, principalmente dos conceitos de bases de dados de documentos, *domain driven design* e *RavenDB*, condicionou o desenvolvimento do modelo de dados, tornando-o um processo iterativo.

A evolução de cada componente, com a alteração das suas dependências, provocou alguns constrangimentos durante o desenvolvimento do projecto. Por exemplo, a framework ASP.NET MVC 4 Web Api e o cliente RavenDB têm uma dependência da biblioteca *Newtonsoft.JSON*, o que se tornou um problema quando as versões utilizadas por cada um deles era diferente.

Uma vez que estes componentes são recentes ainda estão muito propensos a erros. No decorrer do projecto foi detectado um erro na implementação do *bundle* de autorização disponibilizado pelo RavenDB[^ravendberro] o erro já foi corrigido. 


Notas a salientar
-

Uma vez que usamos serviços fornecidos por entidades externas, foi necessário contactar os seus fornecedores para conseguir junto destes a resolução de problemas e licenciamento. 

Como base de dados usada na infra-estrutura é o *RavenDB*, e a instância que usada fornecida pela empresa *RavenHQ*, foi necessário entrar em contacto com os responsáveis do RavenHQ para que configurassem a instância da base de dados usada por nós de acordo com as nossas necessidades. Esta situação também se verificou com o *Mailgun*, sendo que no caso deste nos foi concedida uma licença de uso académico que permitia o envio de um maior número de emails.

Para elaboração das figuras usadas na documentação deste projecto foram feitas usando a aplicação web lucidcharts \cite{lucidcharts}. Uma vez que este é um produto pago, contactamos os seus responsáveis no sentido de nos cederem uma licença, para uso durante o desenvolvimento do projecto, ao que eles acederam.

utilização de frameworks opensource
problemas (bug) com frameworks utilizadas

Trabalho Futuro
-

Apesar de considerarmos que a solução desenvolvida cumpre os objectivos propostos sentimos, no entanto, que esta não tem todas as funcionalidades inicialmente pensadas. Existem aspectos que poderiam ser melhorados, tais como:

 * Planeamento de actividades

	Apesar de ser possível criar dependência entre actividades, essa dependência não se traduz em nenhuma regra ou proibição. Como trabalho futuro podem ser criadas regras para impedir o registo de tempo despendido na realização de tarefas cujas dependências não se encontrem concluídas.

 * Análise e Monitorização 

	Implementar novas formas de mostrar a informação disponível apresentando ao utilizador o estado das actividades e diferenciando as actividades concluídas das restantes.

 * *Timeline*

	Criação de uma página que resume a actividade recente do utilizador durante um período, por exemplo a semana actual, contendo as actividades que lhe estão atribuídas.

 * Repositório de Ficheiros
 
	Criação de um repositório de ficheiros associado aos projectos, onde os seus membros podem alojar informação. Pode também ser implementada a funcionalidade de anexar ficheiros a mensagens de discussões tirando partido desse repositório.
	
[^ravendberro]: O *topic* http://goo.gl/FpIoq no Google Groups do RavenDB detalha o erro e a solução.
