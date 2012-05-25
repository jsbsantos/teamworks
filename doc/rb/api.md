Teamworks Api 
--

A Api do projecto foi desenvolvida sobre o protocolo HTTP e o modelo de arquitectura [ReST][1]. Houve a preocupação de tornar a Api o mais intuitivo possível e todos os recursos do sistema têm um URL de acesso.

Todos os acessos à Api, referidos neste documento, têm como base o seguinte URL:

````
https:\\teamworks.apphb.com/api/v1
````

Autenticação
--

A autenticação na Api é feita utilizando *Basic authentication*. É necessário um *token* de acesso que é atribuido no momento de criação de uma conta na aplicação web Teamworks. 
<span style="background-color: yellow">Indicar onde é possível ver o token.</span>

Para o utilizador ser reconhecido pela Api no pedido tem de enviar como nome de utilizador a palavra 'api' e como password o *token* obtido anteriormente.

É possível testar a autenticação acedendo no browser ao URL:

````
https:\\api:{token}@teamworks.apphb.com/api/v1/person
````

Formato da data
==

<span style="background-color: yellow">RFC 2822</span>

Códigos de resposta
==

Todas as respostas da Api têm o código mais adequado segundo as normas HTTP para descrever o resultado do pedido.

<span style="background-color: yellow">Tabela com códigos que podem ser retornados pela Api. </span>

Obter projectos
=

Para aceder a todos os projectos disponíveis o URL a aceder é o seguinte:

````
get /projects 
````

span style="background-color: red; color: white">To be continued...</span>

[1]: ReST - Representational state transfer



