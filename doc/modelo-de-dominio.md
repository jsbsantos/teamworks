# Modelo de domínio

O modelo de domínio é o conjunto de vários termos, diagramas e conceitos que representam a informação e o comportamento de uma aplicação face a um problema. Para representar o modelo de domínio podem ser usados diagramas UML, texto detelhado, esquemas entidade-associação, use cases, etc.
entre si. Um aspecto importante do modelo de domínio é a sua compreensão por todos os intervenientes no projecto (e.g. arquitectos de software, programadores, cliente). Aos elementos de um modelo de domínio dá-se o nome de objectos de domínio.

Entidades, value objects, agregados, fábricar e repositórios são alguns conceitos transversais a qualquer modelo de domínio
Uma entidade representa um objecto do modelo que tem um identificador unico em todo o seu tempo de vida na aplicação e pode ser acedido usando esse identificador. 

Um value object assim como uma entidade é representado pelas suas características e atributos mas não tem identidade no sistema, ou seja value object com os mesmas caracteristicas e atributos são considerados o mesmo.

No modelo de domínio quando um grupo de objectos são tratados como uma unidade no que diz respeito á informação que estes representão são considerados agregados. Um agregado define uma barreira e tem como raiz uma entidade e é através dessa entidade que os outros elementos do agregado são acedidos. 

As fábricas e os repositórios são usados para gerir o tempo de vida das entidades. As fábricas são usadas na criação e por vezes para abstrair o tipo concreto do objecto criado. Os repositórios são mecanismos que encapsulam a obtenção, armazenamento e a procura dos dados do sistema.

## Teamworks

Para solucionar o problema apresentado por este projecto foram identificadas duas entidades centrais, a pessoa e o projecto.
Cada pessoa pode estar envolvida em vários projectos, assim como um projecto pode ser desenvolvido por várias pessoas. Para representar esta realidade existe a noção de:

 * **Pessoa**, representada pelo username, o email e a password. O email é usado para comunicar com a pessoa e os outros dois atributos servem de autenticação perante o sistema.
 * O **Projecto**,  é o a raíz da infraestrutura e agrega as pessoas que a ele estão associados.

No contexto de um projecto é possível definir Tarefas, outra entidade de domínio. Uma tarefa tem um nome e uma descrição. As Tarefas podem ter várias pessoas associadas que podem registar o tempo que despenderam na sua realização. A Tarefa tem também uma previsão do tempo que deve ser despendido a realiza-la e a data em que deve ser concluída.

## Base de dados

As aplicações actualmente utilizam, na sua maioria, um sistema de gestão de base de dados (SGBD) baseado no modelo relacional.

Neste modelo de dados são conhecidos dois conceitos, o conceito de entidade e o conceito de relação. Uma entidade é um elemento caracterizado pelos seus dados sendo esses dados armazenados num registo de uma tabela. A relação determina o forma como os registos de diferentes tabelas se relacionam entre si. A forma de ler e escrever dados nestes sistemas é através da linguagem SQL.

Para perceber se a melhor opção era utilizar o modelo clássico para a persistência dos dados foram analisadas alternativas 
que surgiram com o movimento NoSQL (Not Only SQL).

### NoSQL

As bases de dados NoSQL diferem do modelo relacional porque são tipicamente desenhadas para escalar horizontalmente e podem ter as seguintes características:

 * Não têm _schema_ fixo;
 * Não suportam operações de join;
 * Suportam o conceito de BASE;
 * Suportam o conceito de CAP;  

Embora algumas destas características possam parecer negativas, existem vantagens na utilização deste tipo de base de dados dependendo da sua categoria. Em NoSQL as bases de dados podem ser categorizadas como key-value stores, base de dados de documentos, base de dados de grafos ou implementações BigTable.

#### Key-value stores 
[referência](http://ayende.com/blog/4449/that-no-sql-thing-key-value-stores)
[referência](http://s3.amazonaws.com/AllThingsDistributed/sosp/amazon-dynamo-sosp2007.pdf)

A função principal de um key-value store é guardar um valor associado a uma chave. Para essa função é disponibilizada uma variação da seguinte API:

```csharp
void Put(string key, byte[] data);
byte[] Get(string key);
void Remove(string key);
```

O valor guardado é um blob. Esta característica torna desnecessária a definição de um _schema_ dando assim total flexibilidade no armazenamento de dados. Devido ao acesso ser feito através de uma chave este tipo de persistência pode facilmente ser optimizada e ter a sua performance melhorada.

 * **Concorrência** - Num key-value store a concorrência apenas se manifesta quando são feitas operações sobre a mesma chave.

 * **Queries** - Uma _query_ é apenas possível fazer com base na chave e como retorno obtêm-se o valor associado. Esta é uma limitação deste tipo de solução mas que se torna uma vantagem em ambientes em que o único tipo de acesso necessário é com base numa chave, como é o case de um sistema de cache.

 * **Transações** - A garantia de que as escritas são feitas no contexto de uma transação só é garantida se for escrita apenas uma chave. É possível oferecer essas garantias para múltiplas chaves mas tendo em conta a natureza de um key-value store de permitir que diferentes chaves estejam armazenadas em diferenter máquinas torna o processo de dificil implementação.

 * **_Schema_** - O _schema_ neste tipo de base de dados é simples, a chave é uma string e o valor é um blob. O tipo e a forma como os dados são estruturados é da responsabilidade do utilizador.

 * **Escalabilidade** - Existem duas formas para o fazer sendo que a mais simples seria separar as chaves. Separar chaves implica decidir a regra de separação, que pode separar as chaves com base no seu primeiro caracter e cada caracter é alojado numa máquina diferente, esta forma torna-se uma não opção quando a máquina onde está a chave não está disponível. Para resolver esse problema é usada replicação.

 * **Replicação** - 

#### Base de dados de documentos
[referencia](http://ayende.com/blog/4459/that-no-sql-thing-document-databases)
[referencia](http://www.ibm.com/developerworks/opensource/library/os-couchdb/index.html#N10062)
[referencia](http://weblogs.asp.net/britchie/archive/2010/08/12/document-databases.aspx)
[referencia](http://highscalability.com/drop-acid-and-think-about-data)

Uma base de dados de documentos é na sua essência um key-value store. A diferença é que, numa base de dados de documentos, o blob de informação é persistido de uma forma semi-estruturada, em documentos, utilizando um formato que possa ser interpretado pela base de dados como JSON, BSON, XML, etc, permitindo realizar queries sobre essa informação.

 * **Concorrência** - Existem várias abordagens para resolver este problema como a concorrência optimista, pessimista ou _merge_. 
  * Concorrência Optimista: Antes de gravar informação é verificado se o documento foi alterado por outra transacção, sendo a transacção abortada nesse caso;
  * Concorrência Péssimista: Usa locks para impedir várias transacções de modificarem o mesmo documento. Esta abordagem é um problema para a escalabilidade destes sistemas;
  * Concorrência _merge_: Semelhante à concorrência optimista mas em vez de abortar a transacção permite ao utilizador resolver o conflito entre as versões do documento.

 * **Transações**- Em alguns casos é dada a garantia de que as operações cumprem com a regra ACID (atomicity, consistency, isolation, durability). Algumas implementações optam por não seguir a regra ACID, desprezando algumas propriedades em detrimento de um aumento de rendimento, usando as regras CAP (Consistency, Availability, Partition Tolerance) ou BASE (Basically Available, Soft State, Eventually Consistent).

 * **_Schema_** - Este tipo de base de dados não necessita que lhe seja definido um _schema à priori_ e não têm tabelas, colunas, tuplos ou relações. Uma base de dados orientada a documentos é composta por vários documentos auto-descritivos, ou seja, a informação relativa a um documento está guardada dentro deste. Isso permite que sejam armazenados objectos complexos (i.e. grafos, dicionários, listas, etc) com facilidade. Esta característica implica que, apesar de poderem existir referências entre documentos a base de dados não garante a integridade dessa relação.

 * **_Queries_** - As _queries_ são feitas com base em índices inferidos automaticamente ou definidos explicitamente pelo programador. Quando o índice é definido o SGBD executa-o e prepara os resultados minimizando o esforço computacional necessário para responder a uma _query_. 

    A forma de actualização destes índices difere em cada implementação deste tipo de base de dados, podendo ser actualizados quando os documentos associados a estes índices são alterados ou no momento anterior à execução da _query_. No primeiro caso isso significa que podem ser obtidos resultados desactualizados, uma vez que as _queries_ aos índices têm resultados imediatos e a actualização pode não estar concluída. Na segunda abordagem, se existirem muitas alterações para fazer a _query_ pode demorar algum tempo a responder.

 * **Escalabilidade** - Este tipo tipo de base de dados suporta Sharding, ou seja partição horizontal, o que permite separar documentos por vários servidores.

 * **Replicação** - 

#### Base de dados de grafos e implementações BigTable

Existem outros tipos de base de dados NoSQL, como o BigTable que é usado pelo Google. 

Tendo em conta o apresentado este projecto utiliza uma base de dados de documentos. A escolha recaiu sobre o RavenDB, uma base de dados de documentos implementada sobre a framework [.NET](http://www.microsoft.com/net). É uma solução transacional, armazena os dados no formato [_JSON_](http://www.json.org/) e para leitura e escrita de dados suporta a utilização da componente [_Linq_](http://msdn.microsoft.com/en-us/library/bb308959.aspx) da framework .Net ou de uma API RESTful disponibilizada através do protocolo HTTP. 
