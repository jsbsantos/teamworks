# API

* como fazer o pedido (url)
* como fazer autentica��o
* media types dispon�veis (apenas json)
* explicar um pouco de HTTP caching, ETAG e os headers associados
* Respostas de erro poss�veis
* endpoints dispon�veis 

## Projectos

(descri��o)

### Obter todos os projectos

 * `GET /projects` retorna todos os projectos.
 
 ````json
 exemplo de uma resposta
 ````

### Obter um projecto

* `GET /projects/{id}` retorna um projecto especifico.

 ````json
 exemplo de uma resposta
 ````

### Criar um projecto

* `POST /projects` cria um projecto com base nos parametros passados.

 ````json
 exemplo de um pedido
 ````

(poss�veis respostas)

### Actualizar um projecto

* `PUT /projects/{id}` actualiza o projecto especificado com base nos parametros passados.

 ````json
 exemplo de um pedido
 ````

(poss�veis respostas)

### Remover um projecto 

* `DELETE /projects/{id}` remove o projecto especificado.

 ````json
 exemplo de um pedido
 ````

(poss�veis respostas)
