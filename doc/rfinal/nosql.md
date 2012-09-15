NoSQL
=

As bases de dados NoSQL diferem do modelo relacional porque são tipicamente desenhadas para escalar horizontalmente e podem ter as seguintes características:

 * Não têm *schema* fixo;
 * Não suportam operações de `JOIN`;
 * Suportam o conceito de BASE[^base];
 * Suportam o conceito de CAP[^CAP];  

[^base]: BASE - **B**asically **A**vailable, **S**oft state, **E**ventual consistency.

[^cap]: CAP - **C**onsitency, **A**vailability, **P**artition tolerance.

Embora algumas destas características possam parecer negativas, existem vantagens na utilização deste tipo de base de dados dependendo da sua categoria \cite{dropacid}.
Em NoSQL as bases de dados podem ser categorizadas como *key-value  stores* \cite{amazonkeyvaluestore} \cite{keyvaluestore}, base de dados de documentos \cite{documentdb} \cite{ibmdocumentdb}, base de dados de grafos ou implementações BigTable.
As base de dados de grafos e implementações BigTable não foram consideras para a realização deste projecto.

Key-value stores
-

A função principal de um key-value store é guardar um valor associado a uma chave. Para essa função é disponibilizada uma variação da Api descrita na lista [valuestore](#):

\lstset{caption={Api simplificada de um key-value store.},label={valuestore}}

````csharp
void Put(string key, byte[] data);
byte[] Get(string key);
void Remove(string key);
````

O valor guardado é um blob. Esta característica torna desnecessária a definição de um *schema* dando assim total flexibilidade no armazenamento de dados. Devido ao acesso ser feito através de uma chave este tipo de persistência pode facilmente ser optimizada e ter a sua performance melhorada.

 + ***Schema*** - O *schema* neste tipo de base de dados é simples, a chave é uma string e o valor é um blob. O tipo e a forma como os dados são estruturados é da responsabilidade do utilizador.

 + **Concorrência** - Num key-value a concorrência apenas se manifesta quando são feitas operações sobre a mesma chave.

 + **Queries** - Uma *query* é apenas possível fazer com base na chave e como retorno obtêm-se o valor associado. Esta é uma limitação deste tipo de solução que não se manifesta em ambientes em que o único tipo de acesso necessário é com base numa chave, como é o case de um sistema de cache.

 + **Escalabilidade** - Existem duas formas para o fazer sendo que a mais simples seria separar as chaves. Separar chaves implica decidir a regra de separação, que pode separar as chaves com base no seu primeiro caracter e cada caracter é alojado numa máquina diferente, esta forma torna-se uma não opção quando a máquina onde está a chave não está disponível. Para resolver esse problema é usada replicação.

 + **Transacções** - A garantia de que as escritas são feitas no contexto de uma transacção só é dada se for escrita apenas uma chave. É possível oferecer essas garantias para múltiplas chaves mas tendo em conta que um key-value store permite que diferentes chaves estejam armazenadas em diferente máquinas torna o processo de difícil implementação.

Base de dados de documentos
-

Uma base de dados de documentos é na sua essência um key-value store. A diferença é que, numa base de dados de documentos, o blob de informação é persistido de uma forma semiestruturada, em documentos, utilizando um formato que possa ser interpretado pela base de dados como JSON, BSON ou XML permitindo realizar *queries* sobre essa informação.

 + ***Schema*** - Este tipo de base de dados não necessita que lhe seja definido um *schema à priori* e não têm tabelas, colunas, tuplos ou relações. Uma base de dados orientada a documentos é composta por vários documentos auto-descritivos, ou seja, a informação relativa a um documento está guardada dentro deste. Isso permite que sejam armazenados objectos complexos (i.e. grafos, dicionários, listas) com facilidade. 
 Esta característica implica que, apesar de poderem existir referências entre documentos a base de dados não garante a integridade dessa relação.

 + **Concorrência** - Existem várias abordagens para resolver este problema como a concorrência optimista, pessimista ou *merge*. 
    + Concorrência Optimista: Antes de gravar informação é verificado se o documento foi alterado por outra transacção, sendo a transacção abortada nesse caso;
	+ Concorrência Pessimista: Usa locks para impedir várias transacções de modificarem o mesmo documento. Esta abordagem é um problema para a escalabilidade destes sistemas;
  	+ Concorrência *merge*: Semelhante à concorrência optimista mas em vez de abortar a transacção permite ao utilizador resolver o conflito entre as versões do documento.

 + **Transacções**- Em alguns casos é dada a garantia de que as operações cumprem com a regra ACID (atomicity, consistency, isolation, durability). Algumas implementações optam por não seguir a regra ACID, desprezando algumas propriedades em detrimento de um aumento de rendimento, usando as regras CAP (Consistency, Availability, Partition Tolerance) ou BASE (Basically Available, Soft State, Eventually Consistent).

 + ***Queries*** - As *queries* são feitas com base em índices inferidos automaticamente ou definidos explicitamente pelo programador. Quando o índice é definido o SGBD executa-o e prepara os resultados minimizando o esforço computacional necessário para responder a uma *query*. 

 	A forma de actualização destes índices difere em cada implementação deste tipo de base de dados, podendo ser actualizados quando os documentos associados a estes índices são alterados ou no momento anterior à execução da *query*.
 	No primeiro caso isso significa que podem ser obtidos resultados desactualizados, uma vez que as *queries* aos índices têm resultados imediatos e a actualização pode não estar concluída. Na segunda abordagem, se existirem muitas alterações para fazer a *query* pode demorar algum tempo a responder.

 + **Escalabilidade** - Este tipo de base de dados suporta Sharding, ou seja partição horizontal, o que permite separar documentos por vários servidores.

RavenDB
-

O RavenDB \cite{ravendb} é uma base de dados de documentos implementada na *framework* .NET \cite{net} que suporta a componente Linq \cite{linq} para *querying*. 
A base de dados é dividida em dois blocos o servidor e o cliente. O servidor é transaccional, armazena os dados no formato JSON e tem como interface um serviço web disponibilizado através do protocolo HTTP. O cliente tem como função expor todas as funcionalidades do servidor através de uma Api. 

###Cliente RavenDB

Para interacção com o cliente são usadas classes *POCO* (*Plain Old CLR Object*) o que torna desnecessária a utilização de um *ORM* ou qualquer sistema de correspondência entre objectos de domínio e os objectos persistidos. O cliente, para além de gerir a comunicação com o servidor, é responsável por fazer cache dos pedidos ao servidor e pela implementação do padrão *Unit of Work*.

A infra-estrutura utiliza principalmente duas classes do cliente, `IDocumentStore` e `IDocumentSession`.
A classe `IDocumentSession` representa uma sessão e permite obter dados, persistir dados e apagar dados da base de dados. O padrão *Unit of Work* é implementado nas instâncias desta classe e é dada a garantia que todas as alterações serão persistidas numa única transacção.
A classe `IDocumentStore` é uma fábrica para a criação de sessões.

###Relações entre documentos

As relações entre documentos, devido à inexistência de operações *JOIN*, podem ser representadas de várias formas. As formas consideradas são a utilização do identificador para incluir as entidades no pedido da entidade principal, a desnormalização e as *live projections*.

**Inclusão no pedido**

Nesta opção cada documento guarda o identificador do(s) documento(s) com que está relacionado (e.g. os documentos que representam projectos guardam o identificador dos documentos que representam tarefas) e quando é obtido um documento principal são também obtidos todos os documentos que lhe estão associadas. 

No RavenDB esta opção é suportada pelo método `Include` que recebe os identificadores das entidades a carregar em paralelo com as entidades principais. As entidades incluídas são adicionadas à sessão pelo cliente RavenDB. 

**Desnormalização**

A desnormalização de um documento consiste em extrair as informações relevantes, de outros documentos, e replicá-las no documento a persistir.

A figura \ref{fig:desnormalizacao} mostra um exemplo de desnormalização. O documento Projecto guarda para além do identificar dos utilizadores que lhe estão associados o seu Nome. Numa situação em que seja necessário apenas o Nome dos utilizadores associados essa informação já está no documento.

![Exemplo de desnormalização de informação\label{fig:desnormalizacao}](http://www.lucidchart.com/publicSegments/view/4fd722d2-6770-4fe6-951d-51600a5705ae/image.png)

Esta situação tem a vantagem de reduzir o número de pedidos à base de dados porque o documento guarda toda a informação necessária.
Esta abordagem tem a desvantagem de que qualquer alteração a um documento desnormalizado implica a alteração de todos os documentos que o utilizam.

***Live Projections***

O *RavenDB* oferece ainda forma de juntar e transformar documentos no servidor obtendo como resultado objectos diferentes dos objectos persistidos. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende.

Na implementação deste projecto optou-se por estabelecer relações entre documentos através do seu identificador, tirando partido da funcionalidade *Include* oferecida pelo cliente RavenDB.

###Bundles

\label{app:ravendb-bundle}

No caso de as funcionalidades disponibilizadas pelo servidor RavenDB não serem suficientes existem Bundles que estendem as funcionalidades oferecidas. 
Os Bundles oferecidos com a build do RavenDB são:

 + **Sharding and Replication**.
 + **Quotas**, coloca limites ao tamanho na base de dados.
 + **Expiration**, remove documentos expirados automaticamente.
 + **Index Replication**, replica índices RavenDB para base de dados SQL Server.
 + **Authentication**, autentica utilizadores na base de dados usando OAuth.
 + **Authorization**, permite a gestão de grupos, *roles* e permissões.
 + **Versioning**, automaticamente gera versões dos documentos quando são alterados ou removidos.
 + **Cascade Deletes**, automatiza operações de remoção *cascade*.
 + **More Like This**, retorna documentos relacionados com o documento indicado.
 + **Unique Constraints**, adiciona a possibilidade de definir *unique constraints* em documentos RavenDB.

Os bundles disponibilizados são distribuídos com dois *dll*s, um para utilizar no cliente e outro para colocar numa pasta definida na configuração do servidor onde são colocadas todas as suas extensões.

###Índices

Para diminuir o tempo de resposta a *queries*, por parte do servidor *RavenDB*, são utilizados índices definidos através de expressões *map-reduce*. 
Os índices estão divididos em duas categorias: índices estáticos e dinâmicos.

Os índices estáticos são criados explicitamente pelo programador, usando sintaxe *LINQ* para definir as suas expressões *map-reduce*, e são definidos de forma permanente na base de dados.

Os índices dinâmicos são criados automaticamente pelo *RavenDB* quando são feitas queries para as quais não existe um índice que dê resposta a essa *query*. O RavenDB não oferece garantia quanto à disponibilidade destes indices pois o seu tempo de vida é gerido pela base de dados. De forma a optimizar a utilização destes índices a base de dados pode optar por transforma-los em índices estáticos.
