Acesso a Dados
=

No ambiente web, que é o usado neste projecto, é frequente que as aplicações necessitem de guardar informação de forma estruturada e organizada. A solução mais frequente é um sistema de gestão de base de dados (SGBD) baseado no modelo relacional. Neste modelo existem dois conceitos principais: o conceito de entidade e o conceito de relação. Uma entidade é um elemento caracterizado pelos seus dados, sendo estes armazenados no registo de uma tabela. A relação determina a forma como essas entidades se relacionam entre si. A forma de ler e escrever dados nestes sistemas é através da linguagem SQL.

Para a realização deste projecto foi usada uma base de dados que não segue o modelo relacional, mas que segue o movimento NoSQL (**N**ot **O**nly **SQL**), descrito no Anexo II, e que é categorizada como uma base de dados de documentos.

A utilização deste tipo de base de dados implica que a modelação das entidades de domínio e das relações entre elas seja definida de forma diferente, pois não existe a noção de operações de *JOIN*. Assim, os documentos que representam entidades têm que conter toda a informação que as caracteriza. Esta característica torna a obtenção de documentos, através da chave da entidade que representam, uma operação quase imediata. 

RavenDB
-

A base de dados usada é o [RavenDB](#ravendb), uma base de dados de documentos implementada sobre a *framework* [.NET](#net) que suporta a utilização de [Linq](#linq), uma componente da *framework* [.NET](#net). 
É uma solução transaccional, que armazena os dados no formato [JSON](#json) e tem como interface um serviço web disponibilizado através do protocolo HTTP.

Devido à inexistência de operações *JOIN*, e ao que foi dito anteriormente, todos os agregados e entidades do domínio são representadas por um documento. 
As representação das relações entre entidades podem ser definidas de várias formas, tendo sido consideradas a utilização do identificador para incluir as entidades no pedido, a desnormalização e as *live projections*.

### Inclusão no pedido
 
Cada entidade guarda o identificador da entidade com que está relacionada (e.g. os projectos guardam o identificador das tarefas) e quando é obtido um projecto são também obtidas todas as tarefas que lhe estão associadas. 

No RavenDB esta opção é suportada pelo método *Include* que recebe os identificadores das entidades a carregar em paralelo com as entidades principais. As entidades incluídas são adicionadas à sessão  

. Ou seja, são carregadas para a *cache* do Raven as entidades pedidas (e.g. Carregar um projecto e as suas tarefas), com apenas um pedido ao servidor (Base de Dados).

### Live Projections

Como complemento para a esta solução o *RavenDB* oferece forma de juntar e transformar documentos, obtendo como resultado objectos personalizados. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende.

### Desnormalização

A desnormalização de uma entidade consiste em extrair as informações relevantes e replicá-los na entidade que necessita deles.

Esta situação tem a vantagem de reduzir o número de pedidos à base de dados porque a entidade guarda internamente a informação relevante.
Esta abordagem tem a desvantagem de que qualquer alteração a uma entidade referenciada implica a alteração de todas as entidades que utilizam dados desnormalizados.

A figura [desnormalizacao](#) mostra um exemplo de desnormalização. A entidade Projecto guarda para além do identificar dos utilizadores que lhe estão associados o seu Nome. Numa situação em que seja necessário apenas o Nome dos utilizadores associados não é necessário obter os dados da base de dados.

![Exemplo de desnormalização de informação\label{desnormalizacao}](https://dl.dropbox.com/s/kno2epnr1hoysex/desnormalizacao.png)


Na implementação deste projecto optou-se por estabelecer relações entre entidades através do seu identificador único, tirando partido da funcionalidade de *Include* oferecida pelo Raven.

Unit of Work
-

De forma a manter registo de alterações a documentos o RavenDB utiliza o padrão *Unit of Work* (Unit of Work)<!---cite-->, implementado sob a forma de uma sessão, através da qual se interage com os documentos. Isto significa que todas as alterações serão persistidas numa única transacção dando ao utilizador a garantia de que os dados ficarão sempre num estado válido após serem gravados. A sessão é utilizada da seguinte forma:

````
var documentStore = new DocumentStore { ConnectionStringName = "RavenDB" }.Initialize();
using (var session = documentStore.OpenSession()) {
    var entity = session.Load<Project>(id);
    entity.Name = "Teamworks";
    session.SaveChanges();
}
````

Repositório
- 

O acesso a documentos é feito através de métodos da Api Cliente do RavenDB. Essa Api consome instâncias das entidades de domínio, implementadas como classes POCO e, como foi dito anteriormente, serializa-as para documentos no formato JSON. Esta característica torna desnecessária a utilização de um ORM ou qualquer sistema de correspondência entre entidades de domínio e o formato dos objectos persistidos. Além disso, a Api Cliente do RavenDB suporta a sua utilização de *LINQ* para fazer *queries* à base de dados tornando o código simples e de fácil leitura.

Tendo em conta o que foi exposto, entendeu-se que não seria necessário criar uma camada de abstracção da forma como é feito o de acesso aos dados(e.g. Repositório). Para esta decisão contribuiu também o facto de toda a infra-estrutura estar fortemente ligada ao RavenDB e a alteração deste SGBD para outro  implicar alteração ao modelo de domínio.

