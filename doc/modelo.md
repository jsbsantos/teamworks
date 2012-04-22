Tento por base o [Modelo de Domínio](https://github.com/isel-leic-ps/LI61N-G07/wiki/Modelo-de-Dom%C3%ADnio) apresentado e o tipo de [Base de Dados](https://github.com/isel-leic-ps/LI61N-G07/wiki/Base-de-Dados) a utilizar foi necessário fazer a modelação das entidades de domínio adequado-as ao uso em base de dados de documentos. Esta necessidade surge pela forma como são definidas relações entre entidades em bases de dados de documentos.

As relações entre entidades podem ser definidas de diversas formas:

### Desnormalização

Uma abordagem possível para este problema é a desnormalização dos dados, ou seja, uma entidade agregadora passa a conter parte (ou a totalidade) da informação das entidades que agrega, em vez de ter apenas uma chave estrangeira para estas. Esta técnica permite que, ao carregar uma entidade agregadora tenhamos disponível parte da informação dos seus filhos, bem com a chave estrangeira para os carregar caso seja necessário agir sobre estes.

### _Includes_

Com a desnormalização surgem problemas com a actualização de dados pelo facto de existir informação fora do documento da entidade a que esta pertence. Actualizar um campo em entidades que são referenciadas por outras tem um custo computacional elevado, para o servidor. 

Para evitar estas situações o RavenDB oferece o método _Include_ que permite carregar entidades, através da sua chave, aquando da execução de uma _query_. Este método carrega a entidade pretendida para a _cache_ o que faz com que acessos a essa entidade não se traduzam em _queries_ ao SGBD.

### _Live Projections_

Como complemento para a solução anterior o RavenDB oferece forma de, na base de dados, juntar documentos e transformar o resultado em objectos customizados. Esta funcionalidade permite carregar documentos relacionados, escolhendo as propriedades de cada um que se pretende carregar.

Das abordagens apresentadas escolheu usar-se a Desnormalização, conseguindo desta forma ter alguma informação sobre as entidades relacionadas, sem ter que as carregar explicitamente. As alterações ao modelo consistem em guardar nas entidades raiz a referência e parte da informação relevante da entidade que esta referencia, em vez de guardar apenas o seu identificador. 