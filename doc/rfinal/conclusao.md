Conclusão
=

A solução *Teamworks* é composta por uma aplicação web e por uma web Api.
A aplicação web tem uma interface simples com o objectivo de minimizar o tempo utilizado na plataforma.
A Api expõe todos os recursos da infra-estrutura permitindo o seu acesso através de diversos clientes (e.g. browser, aplicações móveis, aplicações desktop).

As funcionalidades disponibilizadas permitem a gestão de projectos e pessoas. 
Para a gestão de projectos é possível planear tarefas, estimar a sua duração, atribuir-lhes responsáveis e monitorizar o estado global do projecto. 
As pessoas podem ver as tarefas que têm a realizar e registar o tempo despendido. Para promover a colaboração e partilha de informações é possível pessoas do mesmo projecto e/ou da mesma tarefa criar debates.

Sendo este um projecto académico sentimos que era uma boa oportunidade para aprender e utilizar novas tecnologias. Desta forma, no desenvolvimento deste projecto foram usadas conceitos e tecnologias que não fazem parte do actual plano de estudos do curso de Licenciatura em Engenharia Informática de Computadores, com as quais não estávamos familiarizados e que requereram período de aprendizagem. A aprendizagem e utilização de novas técnicas e tecnologias é vantajosa para o desenvolvimento de competências e alargar o espectro de conhecimento. São exemplos dessas tecnologias a base de dados de documentos *RavenDB*, a *framework javascript knockout* e a *framework* ASP.NET MVC 4 Web Api. 

A necessidade de aprofundar o conhecimento nestas tecnologias, principalmente dos conceitos de bases de dados de documentos, *domain driven design* e *RavenDB*, condicionou o desenvolvimento do modelo de dados, tornando-o um processo iterativo.

Durante o desenvolvimento da infra-estrutura a *framework* ASP.NET MVC 4 Web Api sofreu actualizações o que provocou incompatibilidades com o cliente *Raven*. A *framework* ASP.NET MVC 4 Web Api e o cliente RavenDB têm uma dependência da biblioteca *Json.NET*, o que se tornou um problema pois as versões utilizadas por cada um deles é diferente. 
Para solucionar este problema foi usada uma versão do cliente do *Raven* ainda em *pre-release*, que não depende da biblioteca *Json.NET*.

Consideramos que a solução desenvolvida atinge os objectivos propostos permitindo aos utilizadores fazer diversas acções, como a criação e gestão de projectos, tarefas e debates usando a aplicação web e a Api.

Notas a salientar
-

Uma vez que foram usados serviços fornecidos por entidades externas, foi necessário contactar os seus fornecedores para conseguir junto destes a resolução de problemas e licenciamento. 

Para alojar a aplicação web decidiu usar-se a plataforma *AppHarbor* que suporta a disponibilização de aplicações web, tendo por base o código presente em repositórios *git*. Para automatizar este processo foi configurado um *hook* no *GitHub* que desencadeia o processo de compilação do código e execução dos testes unitários da solução no *AppHarbor*. Se o resultado for positivo é criada uma versão da aplicação web passível de ser disponibilizada.

A base de dados usada na infra-estrutura disponibilizada pela empresa *RavenHQ*. Foi necessário entrar em contacto com os responsáveis do *RavenHQ* para que configurassem na instância de base de dados o *bundle* de autorização. A base de dados é integrada no *AppHarbor* através da utilização de um *addon*.

No caso *Mailgun*, foi atribuído à conta usada pela aplicação o pacote *standard*, que permite o envio de um maior número de emails do que o pacote base, sem qualquer custo.

Durante os desenvolvimento do projecto foram encontrados erros nos componentes utilizados o que condicionou a sua utilização. Um dos erros detectados estava relacionado com a implementação do *bundle* de autorização disponibilizado pelo *Raven*[^ravendberro]. Este erro revelou-se impeditivo para a continuação dos desenvolvimentos da componente de autorização até à disponibilização de uma correcção por parte dos fornecedores. Os outros erros detectados foram resolvidos em tempo útil, pelo que não implicaram constrangimentos nos trabalhos.

Para elaboração das figuras usadas na documentação deste projecto foram feitas usando a aplicação web *LucidChart* \cite{lucidchart}. Uma vez que este é um produto pago, contactamos os seus responsáveis no sentido de nos cederem uma licença, para uso durante o desenvolvimento do projecto, ao que eles acederam.

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
 
	Criação de um repositório de ficheiros associado aos projectos, onde os seus membros podem alojar informação. Pode também ser implementada a funcionalidade de anexar ficheiros a mensagens de debates tirando partido desse repositório.
	
[^ravendberro]: O *topic* http://goo.gl/FpIoq no Google Groups do RavenDB detalha o erro e a solução.
