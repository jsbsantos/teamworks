Web Api 
=

Para tornar a infra-estrutura fléxivel e permitir integração com outros sistemas foi desenvolvida uma Api que permite interagir com o sistema. Esta Api assenta sobre modelo de arquitectura ReST (ReST)<!---cite-->, sobre o protocolo HTTP. No modelo ReST todos os objectos da aplicação são considerados recursos e têm um URL único. Todos os acessos à Api, referidos neste documento, têm como base o URL `https://teamworks.apphb.com/api`.

Autenticação 
-

A autenticação na Api é feita utilizando HTTP *Basic authentication*. 
É possível testar a autenticação acedendo no browser ao URL:

````
https://nome-de-utilizador:password@teamworks.apphb.com/api/person
````

Formato da data
-

O formato da data utilizado pela Api é o descrito no *RFC 2822 (RFC2822)<!---cite--> "Standard for the Format of ARPA Internet Text Messages"*, na secção ".3.3. Date and Time Specification". 


<span style="background-color: yellow">RFC 2822</span>

Códigos de resposta
-

Todas as respostas da Api têm o código mais adequado, segundo as normas HTTP (RFC2616)<!---cite-->, para descrever o resultado do pedido.

<span style="background-color: yellow">Tabela com códigos que podem ser retornados pela Api. </span>

<!---table{| l | l | l |}-->

| **Código de Resposta** | **Descrição** | **Motivo** | 
|-----|-----|-----|
| 404 | Not Found | Recurso não existe |
| 400 | Bad Request | Pedido não está no formato correcto |
| 201 | OK (Created) | Recurso criada com sucesso. |
| 203 | No Content | Resposta enviada sem corpo |
<!---!table{Código de Resposta da Api,tabcodresp}-->

Endereços Disponiveis na Api
-

Para aceder a todos os projectos disponíveis o URL a aceder é o seguinte:

<!---table{| l | l | l |}-->

| **URI** | **Método** | **Descrição** |
|-----|-----|-----|
| api/projects | GET | Obter todos os projectos do utilizador |
| api/projects | POST | Criar novo Projecto |
| api/projects/**{id}** | GET | Obter um projecto existente |
| api/projects/**{id}** | PUT | Editar um projecto |
| api/projects/**{id}** | DELETE | Apagar um projecto |
| api/projects/**{projectid}**/discussions | GET | Obter todos os debates associados ao projecto com o ID passado como parametro |
| api/projects/**{projectid}**/discussions | POST | Criar um novo debate associado ao projecto com o ID passado como parametro |
| api/projects/**{projectid}**/discussions/**{id}** | GET | Obter um debate existente |
| api/projects/**{projectid}**/discussions/**{id}** | PUT | Editar um debate existente |
| api/projects/**{projectid}**/discussions/**{id}** | DELETE | Apagar um debate |
| api/projects/**{projectid}**/tasks | GET | Obter todas as tarefas associadas ao projecto com o ID passado como parametro |
| api/projects/**{projectid}**/tasks | POST | Criar uma nova tarefa associado ao projecto com o ID passado como parametro |
| api/projects/**{projectid}**/tasks/**{id}** | GET | Obter uma tarefa |
| api/projects/**{projectid}**/tasks/**{id}** | PUT | Editar uma tarefa |
| api/projects/**{projectid}**/tasks/**{id}** | DELETE | Apagar uma tarefa |
| api/projects/**{projectid}**/tasks/**{taskid}**/timelog | GET | Obter todos os registos de tempo associados a uma tarefa |
| api/projects/**{projectid}**/tasks/**{taskid}**/timelog | POST | Criar um novo registo de tempo associado à tarefa com o ID passado como parametro |
| api/projects/**{projectid}**/tasks/**{taskid}**/timelog | PUT | Editar um registo de tempo |
| api/projects/**{projectid}**/tasks/**{taskid}**/timelog/**{id}** | DELETE | Apagar um registo de tempo |
| api/projects/**{projectid}**/people | GET |  |
| api/projects/**{projectid}**/people | POST |  |
| api/projects/**{projectid}**/people | PUT |  |
| api/projects/**{projectid}**/people | DELETE |  |
| api/projects/**{projectid}**/discussions/**{discussionid}**/messages | GET | Obter todas as mensagens de um debate associado ao projecto com o ID passado como parametro |
| api/projects/**{projectid}**/discussions/**{discussionid}**/messages | POST | Criar uma nova mensagem de um debate associado ao projecto com o ID passado como parametro |
| api/projects/**{projectid}**/discussions/**{discussionid}**/messages | PUT | Editar uma mensagem de um debate |
| api/projects/**{projectid}**/discussions/**{discussionid}**/messages/**{id}** | DELETE | Apagar uma mensagem de um debate |

<!---!table{Endereços da Api e respectivos códigos de retorno,tabcodret}-->
<!---T:FloatBarrier-->

<span style="background-color: red; color: white">To be continued...</span>

[1] ReST - Representational state transfer

Unit of Work
-

Para cada pedido feito à Api disponibilizada é, normalmente, necessário fazer o acesso à Base de Dados. Esse acesso é feito através do uso de uma sessão (*DocumentSession*), obtida através da Api Cliente do RavenDB. Para facilitar a criação e destruição desta sessão, por cada pedido, foi criada uma classe que é responsável por toda a gestão e criação de sessões. 

Esta classe, implementada usando o padrão Singleton, é responsável por instânciar a fábrica *(factory)* de sessões (*DocumentStore*), que faz parte da Api Cliente RavenDB, e disponibiliza métodos e propiedades para obtenção de instâncias de sessões *DocumentSession* e abertura e destruição das mesmas.

````
public class Raven
    {
        private const string Key = "RAVEN_CURRENT_SESSION_KEY";

        private static readonly Lazy<Raven> _instance = new Lazy<Raven>(() => new Raven());

        public readonly IDocumentStore Store;

        private Raven()
        {
            Store = new DocumentStore
                        {
                            ConnectionStringName = "RavenDB"
                        }.Initialize();
        }

        public static Raven Instance
        {
            get { return _instance.Value; }
        }

        public IDocumentSession Open() { ... }

        public void TryOpen() { ... }
		
        public IDocumentSession CurrentSession
        {
            get { ... }
        }

    }
```` 



