Registo 
=

Para utilizar a aplicação é necessário que o utilizador se registe. No registo o utilizador indica o email, usado pela aplicação como forma de comunicar; o nome de utilizador, para o identificar; e a password que em conjunto com o nome de utilizador é usada para autenticar o utilizador.

Na base de dados é guardado o *hash* da *password*. O *hash* é gerado usando um algoritmo de dispersão (SHA-256) que tem como entrada a concatenação da password com um salto aleatório.

Autenticação
=

A autenticação na aplicação é feita usando o nome de utilizador e a *password*. O nome de utilizador é usado para obter a informação da Person a autenticar (people/nome-de-utilizador). A autenticação é validade se o resultado da função de dispersão usada no resgisto for igual ao obtido. A função de dispersão tem como parâmetro de entrada a concatenação da *password* inserida com o salto presente na Person obtida, tendo sido gerado no momento em que o utilizador se registou.

Autorização
=





