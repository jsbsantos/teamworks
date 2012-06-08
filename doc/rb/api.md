Web Api 
=

A Api da infra estrutura é desenvolvida usando modelo de arquitectura ReST[[1]](http://link/) sobre o protocolo HTTP. No modelo ReST todos os objectos da aplicação são considerados recursos e têm um URL único. Todos os acessos à Api, referidos neste documento, têm como base o URL `https://teamworks.apphb.com/api`.

Autenticação
-

A autenticação na Api é feita utilizando HTTP *Basic authentication*. 
É possível testar a autenticação acedendo no browser ao URL:

````
https://nome-de-utilizador:password@teamworks.apphb.com/api/person
````

Formato da data
=

<span style="background-color: yellow">RFC 2822</span>

Códigos de resposta
=

Todas as respostas da Api têm o código mais adequado segundo as normas HTTP para descrever o resultado do pedido.

<span style="background-color: yellow">Tabela com códigos que podem ser retornados pela Api. </span>

<!---table{| l | l | l |}-->

| **Código de Resposta** | **Descrição** | **Motivo** | 
|-----|-----|-----|
| 404 | Not Found | Recurso não existe |
| 400 | Bad Request | Pedido não está no formato correcto |
| 201 | OK (Created) | Recurso criada com sucesso. |
| 203 | No Content | Resposta enviada sem corpo |
<!---!table{Código de Resposta da Api,tabcodresp}-->

Obter projectos
=

Para aceder a todos os projectos disponíveis o URL a aceder é o seguinte:

<!---table{| l | l | l |}-->

| **URI** | **Método** | **Descrição** |
|-----|-----|-----|
| api/projects | GET |  |
| api/projects | POST |  |
| api/projects | PUT |  |
| api/projects | DELETE |  |
| api/projects/{id} | GET |  |
| api/projects/{id} | POST |  |
| api/projects/{id} | PUT |  |
| api/projects/{id} | DELETE |  |
| api/projects/{projectid}/discussions | GET |  |
| api/projects/{projectid}/discussions | POST |  |
| api/projects/{projectid}/discussions | PUT |  |
| api/projects/{projectid}/discussions | DELETE |  |
| api/projects/{projectid}/discussions/{id} | GET |  |
| api/projects/{projectid}/discussions/{id} | POST |  |
| api/projects/{projectid}/discussions/{id} | PUT |  |
| api/projects/{projectid}/discussions/{id} | DELETE |  |
| api/projects/{projectid}/tasks | GET |  |
| api/projects/{projectid}/tasks | POST |  |
| api/projects/{projectid}/tasks | PUT |  |
| api/projects/{projectid}/tasks | DELETE |  |
| api/projects/{projectid}/tasks/{id} | GET |  |
| api/projects/{projectid}/tasks/{id} | POST |  |
| api/projects/{projectid}/tasks/{id} | PUT |  |
| api/projects/{projectid}/tasks/{id} | DELETE |  |
| api/projects/{projectid}/tasks/{taskid}/timelog | GET |  |
| api/projects/{projectid}/tasks/{taskid}/timelog | POST |  |
| api/projects/{projectid}/tasks/{taskid}/timelog | PUT |  |
| api/projects/{projectid}/tasks/{taskid}/timelog | DELETE |  |
| api/projects/{projectid}/tasks/{taskid}/timelog/{id} | GET |  |
| api/projects/{projectid}/tasks/{taskid}/timelog/{id} | POST |  |
| api/projects/{projectid}/tasks/{taskid}/timelog/{id} | PUT |  |
| api/projects/{projectid}/tasks/{taskid}/timelog/{id} | DELETE |  |
| api/projects/{projectid}/people | GET |  |
| api/projects/{projectid}/people | POST |  |
| api/projects/{projectid}/people | PUT |  |
| api/projects/{projectid}/people | DELETE |  |
| api/projects/{projectid}/discussions/{discussionid}/messages | GET |  |
| api/projects/{projectid}/discussions/{discussionid}/messages | POST |  |
| api/projects/{projectid}/discussions/{discussionid}/messages | PUT |  |
| api/projects/{projectid}/discussions/{discussionid}/messages | DELETE |  |
| api/projects/{projectid}/discussions/{discussionid}/messages/{id} | GET |  |
| api/projects/{projectid}/discussions/{discussionid}/messages/{id} | POST |  |
| api/projects/{projectid}/discussions/{discussionid}/messages/{id} | PUT |  |
| api/projects/{projectid}/discussions/{discussionid}/messages/{id} | DELETE |  |

<!---!table{Endereços da Api e respectivos códigos de retorno,tabcodret}-->

<span style="background-color: red; color: white">To be continued...</span>

[1] ReST - Representational state transfer

Unit of Work
-

Para cada pedido feito à Api disponibilizada é, normalmente, necessário fazer o acesso à Base de Dados. 


