Acesso a Dados
=

As aplicações que necessitam de persistir informação de forma estruturada e organizada utilizam, na sua maioria, um sistema de gestão de base de dados (SGBD) baseado no modelo relacional. Neste modelo existem dois conceitos principais: o conceito de entidade e o conceito de relação. Uma entidade é um elemento caracterizado pelos seus dados, sendo estes armazenados no registo de uma tabela. A relação determina a forma como essas entidades se relacionam entre si. A forma de ler e escrever dados nestes sistemas é através da linguagem SQL.

Para a realização deste projecto foi usada uma base de dados que não segue o modelo relacional, mas que segue o movimento NoSQL (**N**ot **O**nly **SQL**), descrito no Anexo II, e que é categorizada como uma base de dados de documentos.

A utilização deste tipo de base de dados implica que a modelação das entidades de domínio e das relações entre elas seja definida de forma diferente, pois não existe a noção de operações de *JOIN*. Assim, os documentos que representam entidades, têm que conter toda a informação que as caracteriza. Esta caracteristica torna a obtenção de documentos, através da chave da entidade que representam, uma operação quase imediata. 

RavenDB
-

A base de dados usada é o [RavenDB](#ravendb), uma base de dados de documentos implementada sobre a *framework* [.NET](#net) que suporta a utilização de [Linq](#linq), uma componente da *framework* [.NET](#net). É uma solução transaccional, que armazena os dados no formato [JSON](#json) e tem como interface um serviço web disponibilizado através do protocolo HTTP

Devido à inexistência de operações *JOIN*, e ao que foi dito anteriormente, todos os agregados e entidades do domínio são representadas por um documento. As representação das relações entre entidades podem ser definidas de várias formas, tendo sido consideradas as seguintes:

* Desnormalização

A desnormalização de uma entidade consiste em guardar parte da informação que a caracteriza, em vez de guardar apenas o seu identificador. 

Numa situação em que uma entidade referencia outras e necessita de parte dos seus dados o carregamento de várias entidades pode representar um grande volume de dados. Para solucionar este problema a entidade referenciada é desnormalizada e são carregados apenas o dados relevantes juntamente com a entidade referenciadora.

Contudo, alterações aos de uma entidade implicam que os dados desnormalizados dessa entidade também sejam alterados.

A figura [desnormalizacao](#) mostra um exemplo de desnormalização de informação: a entidade Projecto guarda parte da informação dos utilizadores que lhe estão associados.

<!---figure-->

![Exemplo de desnormalização de informação\label{desnormalizacao}](https://dl.dropbox.com/s/kno2epnr1hoysex/desnormalizacao.png)<!--- desnormalizacao -->

<!---!figure-->

* *Identificador*
 
Cada entidade guarda apenas o identificador único da entidade com que está relacionada (e.g. os projectos guardam o identificador das tarefas que lhe estão associadas). Desta forma evita-se o custo da actualização de dados desnormalizados.

Este tipo de representação de relação é suportado, no RavenDB, pelo método *Include* que permite carregar entidades, através do seu identificador, na execução de uma *query*. Ou seja, são carregadas para a *cache* do Raven as entidades pedidas (e.g. Carregar um projecto e as suas tarefas), com apenas um pedido ao servidor (Base de Dados).

**Live Projections**

Como complemento para a esta solução o *RavenDB* oferece forma de juntar e transformar documentos, obtendo como resultado objectos personalizados. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende.


Na implentação deste projecto optou-se por estabelecer relações entre entidades através do seu identificador único, tirando partido da funcionalidade de *Include* oferecida pelo Raven.

Unit of Work
-

De forma a manter registo de alterações a documentos o RavenDB utiliza o padrão *Unit of Work* (Unit of Work)<!---cite-->, implementado sob a forma de uma sessão, através da qual se interage com os documentos. Isto significa que todas as alterações serão persistidas numa unica transacção dando ao utilizador a garantia de que os dados ficarão sempre num estado válido após serem gravados. A sessão é utilizada da seguinte forma:

````
using (var session = documentStore.OpenSession()) {
    var entity = session.Load<Company>(companyId);
    entity.Name = "Another Company";
    session.SaveChanges();
}
````

Repositório
- 

O acesso a documentos é feito através de métodos da Api Cliente do RavenDB. Essa Api consome instâncias das entidades de domínio, implementadas como classes POCO e, como foi dito anteriormente, serializa-as para documentos no formato JSON. Esta caracteristica torna desnecessária a utilização de um ORM ou qualquer sistema de correspondência entre entidades de dominio e o formato dos objectos persistidos. Além disso, Api Cliente do RavenDB suporta a sua utilização de *LINQ* para fazer *queries* à base de dados tornando o código simples e de facil leitura.

Tendo em conta o que foi exposto, entendeu-se que não seria necessário criar uma camada de abstracção da forma como é feito o de acesso aos dados(e.g. Repositório). Para esta decisão contribuiu também o facto de toda a infra-estrutura estar fortemente ligada ao RavenDB e a alteração deste SGBD para outro  implicar alteração ao modelo de domínio.

