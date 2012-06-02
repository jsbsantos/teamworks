Registo
=

Para aceder às funcionalidades da aplicação o utilizador tem de se registar na aplicação e indicar o nome de utilizador, a password e um email válido.

![Figura - Formulário de registo.](http://imagem/)

Na base de dados é guardado o resultado de uma função de *hash* que tem como parâmetro de entrada a *password*.
 
```Explicar o que faz o salto.```

Autenticação
=

Para se autenticar na aplicação o utilizador indica o nome de utilizador e a password escolhidos no registo. 

```Processo de verificação de password.```

Para manter o utilizador autenticado são usadas as classes FormsAuthenticationModule e FormsAuthentication disponíveis na *framework .NET*[[1]](http://link/). 
