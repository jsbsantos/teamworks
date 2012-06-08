Acesso a Dados
=

Introdução
-
As aplicações utilizam, na sua maioria, um sistema de gestão de base de dados (SGBD) baseado no modelo relacional para persistência de dados. Neste modelo de dados existem dois conceitos principais: o conceito de entidade e o conceito de relação. Uma entidade é um elemento caracterizado pelos seus dados, sendo estes armazenados no registo de uma tabela. A relação determina a forma como os registos de diferentes tabelas se relacionam entre si. A forma de ler e escrever dados nestes sistemas é através da linguagem SQL.

Para a realização deste projecto foi usada uma base de dados que não segue este modelo. A base de dados utilizada segue o movimento NoSQL (**N**ot **O**nly **SQL**), descrito no Anexo II, e que é categorizada como uma base de dados de documentos.

RavenDB
-

A base de dados usada é o *RavenDB* *(ravendb)*<!---cite-->, uma base de dados de documentos implementada sobre a *framework .NET* *(.net)*<!---cite--> que suporta a utilização de *Linq* *(linq)*<!---cite-->, uma componente da *framework .NET* *(net)*<!---cite-->. É uma solução transaccional, que armazena os dados no formato *JSON* *(json)*<!---cite--> e tem como interface um serviço web disponibilizado através do protocolo HTTP.

A utilização deste tipo de base de dados implica que a modelação das entidades de domínio e das relações entre elas seja definida de forma diferente, pois não existe a noção de operações de *JOIN*. Assim, os documentos que representam entidades, têm que conter toda a informação que as caracteriza.

Devido a isso todos os agregados/***entidades*** do domínio são representadas por um documento. As representação das relações entre entidades podem ser definidas de várias formas, tendo sido consideradas as seguintes:

* Desnormalização

A desnormalização de uma entidade consiste em guardar parte da informação que a caracteriza, em vez de guardar apenas o seu identificador. 

Numa situação em que uma entidade referencia outras e necessita de parte dos seus dados o carregamento de várias entidades pode representar um grande volume de dados. Para solucionar este problema a entidade referenciada é desnormalizada e são carregados apenas o dados relevantes juntamente com a entidade referenciadora.

Contudo, alterações aos de uma entidade implicam que os dados desnormalizados dessa entidade também sejam alterados.

* *Identificador*

Cada entidade guarda apenas o identificador único da entidade que está relacionada (e.g. os projectos guardam o identificador das tarefas que lhe estão associadas). Desta forma evita-se o custo da actualização de dados desnormalizados.

Este tipo de representação de relação é suportado pelo método *Include* que permite carregar várias entidades, através do seu identificador, no momento da execução de uma *query*. Ou seja, são carregadas para a *cache* do Raven as entidades pedidas (e.g. Carregar um projecto e as suas tarefas), com apenas um pedido ao servidor (Base de Dados).

***Live Projections***

Como complemento para a solução anterior o *RavenDB* *(ravendb)*<!---cite--> oferece forma de juntar e transformar documentos, obtendo como resultado objectos personalizados. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende.


Na implentação deste projecto optou-se por estabelecer relações entre entidades através do seu identificador único, tirando partido da funcionalidade *Include* oferecida pelo Raven.

Unit of Work
-

De forma a manter registo de alterações a documentos o RavenDB utiliza o padrão Unit of Work, implementado sob a forma de uma sessão, através da qual se interage com os documentos. Isto significa que todas as alterações serão persistidas numa transacção dando ao utilizador a garantia de que os dados estão sempre num estado válido. A sessão é utilizada da seguinte forma:

````
using (var session = documentStore.OpenSession())
{
    var entity = session.Load<Company>(companyId);
    entity.Name = "Another Company";
    session.SaveChanges(); // will send the change to the database
}
````

Repositório
- 

Uma vez que o RavenDB permite a sua utilização através de *queries LINQ*, entendeu-se que não seria necessária criar uma camada explicita de acesso a dados. Outra razão que contribuiu para esta decisão foi o facto de toda a infra-estrutura estar fortemente ligada ao RavenDB e que, por isso, a alteração deste SGBD para outro iria implicar alteração ao modelo de domínio.

O acesso a documentos é feito através de métodos da Api Cliente do RavenDB. Essa Api consome instâncias das entidades de domínio implementadas como classes POCO e, como foi dito anteriormente, serializa-as para documentos no formato JSON. Esta caracteristica torna desnecessária a utilização de um ORM ou qualquer sistema de correspondência entre entidades de dominio e o formato dos objectos persistidos.