Modelo de Dados
=

No desenvolvimento de software é frequente que as aplicações guardem grandes volumes de informação de forma estruturada e organizada. A opção mais usual é a utilizadação de um SGBD^[SGBD - Sistema de gestão de base de dados.] baseado no modelo relacional. 
Este modelo tem dois conceitos principais: o conceito de entidade e de relação. 
Uma entidade é um elemento caracterizado pelos seus dados e são armazenados na linha de uma tabela. 
Uma relação determina a forma como essas entidades se relacionam entre si. 
Para interacção com este tipo de sistemas é habitual o uso da linguagem *SQL*.

Para a realização deste projecto foi usada uma base de dados que não segue o modelo relacional. 
A base de dados utilizada é o RavenDB ([anexo1](#)) e segue o movimento *NoSQL* ([anexo2](#)).

Modelo
-

Autorização
-

RavenDB Authorization Bundle

![Diagrama UML de autorização\label{autorizacao}](http://www.lucidchart.com/publicSegments/view/4fd773c2-23b4-4f9e-bea5-7a420adcb320/image.png)