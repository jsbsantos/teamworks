Conclusão
=

A solução *Teamworks* é composta por uma aplicação web e por uma web Api. A Api expõe todos os recursos da infra-estrutura permitindo o seu acesso através de diversos clientes (e.g. browser, aplicações móveis, aplicações desktop).
A aplicação web tem uma interface simples com o objectivo de minimizar o tempo utilizado na plataforma.

As funcionalidades disponibilizadas permitem a gestão de projectos e pessoas. 
Para a gestão de projectos é possível planear tarefas, estimar a sua duração, atribuir-lhes responsáveis e monitorizar o estado global do projecto. 
As pessoas podem ver as tarefas que têm a realizar e registar o tempo despendido. Para promover a colaboração e partilha de informações é possível pessoas do mesmo projecto e/ou da mesma tarefa criar discussões.

No desenvolvimento deste projecto foram usadas técnicas e tecnologias que não fazem parte do actual plano de estudos do curso de Licenciatura em Engenharia Informática de Computadores, com as quais não estávamos familiarizados e que requereram período de aprendizagem. Sendo este um projecto académico sentimos que era uma boa oportunidade para aprender e utilizar novas tecnologias. A aprendizagem e utilização de novas técnicas e tecnologias é vantajosa para o desenvolvimento de competências e alargar o espectro de conhecimento. São exemplos dessas tecnologias a base de dados de documentos *RavenDB*, a framework javascript *knockout* e a framework ASP.NET MVC 4 Web Api. 

A necessidade de aprofundar o conhecimento nestas tecnologias, principalmente dos conceitos de bases de dados de documentos, *domain driven design* e *RavenDB*, condicionou o desenvolvimento do modelo de dados, tornando-o um processo iterativo.

Durante o desenvolvimento da infra-estrutura a framework ASP.NET MVC 4 Web Api sofreu actualizações o que provocou incompatibilidades com o cliente RavenDb. A framework ASP.NET MVC 4 Web Api e o cliente RavenDB têm uma dependência da biblioteca *Json.NET*, o que se tornou um problema quando as versões utilizadas por cada um deles era diferente. 

Consideramos que a solução desenvolvida atinge os objectivos propostos permitindo aos utilizadores fazer diversas acções, como a criação e gestão de projectos, tarefas e debates usando a aplicação web e a Api.

Notas a salientar
-

Uma vez que foram usados serviços fornecidos por entidades externas, foi necessário contactar os seus fornecedores para conseguir junto destes a resolução de problemas e licenciamento. 

Como base de dados usada na infra-estrutura é o *RavenDB*, e a instância que usada fornecida pela empresa *RavenHQ*, foi necessário entrar em contacto com os responsáveis do *RavenHQ* para que configurassem a instância da base de dados usada por nós de acordo com as nossas necessidades. 

Esta situação também se verificou com o *Mailgun*, sendo que no caso deste nos foi concedida uma licença de uso académico que permitia o envio de um maior número de emails.

Para alojar a aplicação web decidiu usar-se a plataforma *AppHarbor* que suporta a disponibilização de aplicações web, tendo por base o código presente em repositórios *git*. Para automatizar este processo foi configurado um *hook* no *GitHub* que desencadeia o processo de compilação do código e execução dos testes unitários da solução no *AppHarbor*. Se o resultado for positivo é criada uma versão da aplicação web passível de ser disponibilizada.

Para elaboração das figuras usadas na documentação deste projecto foram feitas usando a aplicação web *LucidChart* \cite{lucidchart}. Uma vez que este é um produto pago, contactamos os seus responsáveis no sentido de nos cederem uma licença, para uso durante o desenvolvimento do projecto, ao que eles acederam.

Durante os desenvolvimento do projecto foram encontrados erros nos componentes utilizados, o que condicionou a sua utilização. Um dos erros detectados estava relacionado com a implementação do *bundle* de autorização disponibilizado pelo *RavenDB*[^ravendberro]. Este erro revelou-se impeditivo para a continuação dos desenvolvimentos da componente de autorização até à disponibilização de uma correcção por parte do fornecedor.

Trabalho Futuro
-

Apesar de considerarmos que a solução desenvolvida cumpre os objectivos propostos sentimos que esta não tem todas as funcionalidades inicialmente idealizadas. Existem aspectos que poderiam ser melhorados, tais como:

 * Planeamento de actividades

	Apesar de ser possível criar dependência entre actividades, essa dependência não se traduz em nenhuma regra ou proibição. Como trabalho futuro podem ser criadas regras para impedir o registo de tempo despendido na realização de tarefas cujas dependências não se encontrem concluídas.

 * Análise e Monitorização 

	Implementar novas formas de mostrar a informação disponível apresentando ao utilizador o estado das actividades e diferenciando as actividades concluídas das restantes.

 * *Timeline*

	Criação de uma página que resume a actividade recente do utilizador durante um período, por exemplo a semana actual, contendo as actividades que lhe estão atribuídas.

 * Repositório de Ficheiros
 
	Criação de um repositório de ficheiros associado aos projectos, onde os seus membros podem alojar informação. Pode também ser implementada a funcionalidade de anexar ficheiros a mensagens de discussões tirando partido desse repositório.
	
[^ravendberro]: O *topic* http://goo.gl/FpIoq no Google Groups do RavenDB detalha o erro e a solução.

