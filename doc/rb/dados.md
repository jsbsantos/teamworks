Modelo de Dados
=

No desenvolvimento de software é frequente que as aplicações guardem grandes volumes de informação de forma estruturada e organizada. A opção mais usual é a utilizadação de um SGBD^[SGBD - Sistema de gestão de base de dados.] baseado no modelo relacional. 
Este modelo tem dois conceitos principais: o conceito de entidade e de relação. 
Uma entidade é um elemento caracterizado pelos seus dados e são armazenados na linha de uma tabela. 
Uma relação determina a forma como essas entidades se relacionam entre si. 
Para interacção com este tipo de sistemas é habitual o uso da linguagem *SQL*.

Para a realização deste projecto foi usada uma base de dados que não segue o modelo relacional. 
A base de dados utilizada é o RavenDB ([anexo1](#)) e segue o movimento *NoSQL*, descrito no [anexo2](#).

Modelo
-

A utilização de uma base de dados de documentos como meio de persistir informação implica que a modelação das entidades tenha que ter em atenção alguns aspectos particulares. A inexistência de operações *JOIN* neste tipo de base de dados implica que cada entidade seja modelada de forma a conter toda a informação que a caracteriza, sendo persistida em apenas um documento. 
Esta abordagem permite que o carregamento de entidades (uma por documento) seja feito de forma quase imediata.
No entanto, esta caracteristica não implica que não existam relações entre documentos. Existem várias formas de representar estas relações, que serão discutidas mais à frente, no capitulo [RavenDB](#).

.....

A figura [diagramadeclassesmodelo](#) representa o modelo de dados da solução, através de um diagrama de classes UML.

![Diagrama UML de classes do modelo de dados\label{diagramadeclassesdominio}](http://www.lucidchart.com/publicSegments/view/4fd91524-d53c-4604-9e14-42450a4022d4/image.png)
 
A implementação das relações entre entidades^[[Domain-Driven Design](#ddd)] e agregados^[[Domain-Driven Design](#ddd)] é feita através de um identificador único. A relação destes com *value objects*, no entanto, é feita através de inclusão de instâncias do tipo que os representa.

.....

Autorização
-

RavenDB Authorization Bundle

![Diagrama UML de autorização\label{autorizacao}](http://www.lucidchart.com/publicSegments/view/4fd773c2-23b4-4f9e-bea5-7a420adcb320/image.png)