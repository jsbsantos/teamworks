NoSQL
=

As bases de dados NoSQL diferem do modelo relacional porque são tipicamente desenhadas para escalar horizontalmente e podem ter as seguintes características:

 * Não têm *schema* fixo;
 * Não suportam operações de join;
 * Suportam o conceito de BASE;
 * Suportam o conceito de CAP;  

Embora algumas destas características possam parecer negativas, existem vantagens na utilização deste tipo de base de dados dependendo da sua categoria. Em NoSQL as bases de dados podem ser categorizadas como key-value stores, base de dados de documentos, base de dados de grafos ou implementações BigTable. As base de dados de grafos e implementações BigTable não foram consideras neste projecto.

Key-value stores
-

[referência](http://ayende.com/blog/4449/that-no-sql-thing-key-value-stores)
[referência](http://s3.amazonaws.com/AllThingsDistributed/sosp/amazon-dynamo-sosp2007.pdf)

A função principal de um key-value store é guardar um valor associado a uma chave. Para essa função é disponibilizada uma variação da api descrita na lista [valuestore](#):

````[Api simplificada de um key-value store.](valuestore)
void Put(string key, byte[] data);
byte[] Get(string key);
void Remove(string key);
````

O valor guardado é um blob. Esta característica torna desnecessária a definição de um *schema* dando assim total flexibilidade no armazenamento de dados. Devido ao acesso ser feito através de uma chave este tipo de persistência pode facilmente ser optimizada e ter a sua performance melhorada.

 * **Concorrência** - Num key-value store a concorrência apenas se manifesta quando são feitas operações sobre a mesma chave.

 * **Queries** - Uma *query* é apenas possível fazer com base na chave e como retorno obtêm-se o valor associado. Esta é uma limitação deste tipo de solução mas que se torna uma vantagem em ambientes em que o único tipo de acesso necessário é com base numa chave, como é o case de um sistema de cache.

 * **Transações** - A garantia de que as escritas são feitas no contexto de uma transação só é garantida se for escrita apenas uma chave. É possível oferecer essas garantias para múltiplas chaves mas tendo em conta a natureza de um key-value store de permitir que diferentes chaves estejam armazenadas em diferenter máquinas torna o processo de dificil implementação.

 * ***Schema*** - O *schema* neste tipo de base de dados é simples, a chave é uma string e o valor é um blob. O tipo e a forma como os dados são estruturados é da responsabilidade do utilizador.

 * **Escalabilidade** - Existem duas formas para o fazer sendo que a mais simples seria separar as chaves. Separar chaves implica decidir a regra de separação, que pode separar as chaves com base no seu primeiro caracter e cada caracter é alojado numa máquina diferente, esta forma torna-se uma não opção quando a máquina onde está a chave não está disponível. Para resolver esse problema é usada replicação.

 * **Replicação** - 

Base de dados de documentos
-

[referencia](http://ayende.com/blog/4459/that-no-sql-thing-document-databases)
[referencia](http://www.ibm.com/developerworks/opensource/library/os-couchdb/index.html#N10062)
[referencia](http://weblogs.asp.net/britchie/archive/2010/08/12/document-databases.aspx)
[referencia](http://highscalability.com/drop-acid-and-think-about-data)

Uma base de dados de documentos é na sua essência um key-value store. A diferença é que, numa base de dados de documentos, o blob de informação é persistido de uma forma semi-estruturada, em documentos, utilizando um formato que possa ser interpretado pela base de dados como JSON, BSON, XML, etc, permitindo realizar queries sobre essa informação.

 * **Concorrência** - Existem várias abordagens para resolver este problema como a concorrência optimista, pessimista ou *merge*. 
  * Concorrência Optimista: Antes de gravar informação é verificado se o documento foi alterado por outra transacção, sendo a transacção abortada nesse caso;
  * Concorrência Péssimista: Usa locks para impedir várias transacções de modificarem o mesmo documento. Esta abordagem é um problema para a escalabilidade destes sistemas;
  * Concorrência *merge*: Semelhante à concorrência optimista mas em vez de abortar a transacção permite ao utilizador resolver o conflito entre as versões do documento.

 * **Transações**- Em alguns casos é dada a garantia de que as operações cumprem com a regra ACID (atomicity, consistency, isolation, durability). Algumas implementações optam por não seguir a regra ACID, desprezando algumas propriedades em detrimento de um aumento de rendimento, usando as regras CAP (Consistency, Availability, Partition Tolerance) ou BASE (Basically Available, Soft State, Eventually Consistent).

 * ***Schema*** - Este tipo de base de dados não necessita que lhe seja definido um *schema à priori* e não têm tabelas, colunas, tuplos ou relações. Uma base de dados orientada a documentos é composta por vários documentos auto-descritivos, ou seja, a informação relativa a um documento está guardada dentro deste. Isso permite que sejam armazenados objectos complexos (i.e. grafos, dicionários, listas, etc) com facilidade. Esta característica implica que, apesar de poderem existir referências entre documentos a base de dados não garante a integridade dessa relação.

 * ***Queries*** - As *queries* são feitas com base em índices inferidos automaticamente ou definidos explicitamente pelo programador. Quando o índice é definido o SGBD executa-o e prepara os resultados minimizando o esforço computacional necessário para responder a uma *query*. 

A forma de actualização destes índices difere em cada implementação deste tipo de base de dados, podendo ser actualizados quando os documentos associados a estes índices são alterados ou no momento anterior à execução da *query*. No primeiro caso isso significa que podem ser obtidos resultados desactualizados, uma vez que as *queries* aos índices têm resultados imediatos e a actualização pode não estar concluída. Na segunda abordagem, se existirem muitas alterações para fazer a *query* pode demorar algum tempo a responder.

 * **Escalabilidade** - Este tipo tipo de base de dados suporta Sharding, ou seja partição horizontal, o que permite separar documentos por vários servidores.