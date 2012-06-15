Conclusão
=

Trabalho Futuro
-

A versão actual da infra-estrutura apenas permite fazer edição básica de conteúdos. Como trabalho futuro pretende-se implementar funcionalidades que complementem as que já estão disponíveis. As funcionalidades que se pretende implementar são descritas nesta secção.

* Planeamento de tarefas

Serão adicionadas funcionalidades à gestão de tarefas, permitindo atribuir-lhes prioridades e dependências de outras, de forma a permitir um maior controlo e organização sobre às tarefas de um projecto.


* Análise e Monitorização 

Pretende-se que o responsável por um projecto seja capaz de, em qualquer altura, avaliar qual o estado em que este se encontra. Desta forma, é necessário que sejam implementados indicadores para análise de informação sobre o projecto, as suas tarefas e utilizadores.

* *Dashboard*

Deverá estar disponível aos utilizadores informação sobre quais os projectos e respectivas tarefas que lhe estão atribuídas para um período de tempo (e.g. semana actual). Como complemento deve estar disponível a cada utilizador uma lista de *todo*s onde este poderá acrescentar entradas e marcar existentes como concluídas.


Além de novas funcionalidades existem aspectos da solução que devem ser melhorados. 
Devem ser feitos testes unitários e de desempenho(?) da Api e da aplicação web, de forma a despistar possíveis erros. 
A forma de autenticação usada na api web é pouco segura - apenas codifica o nome do utilizador e password em base64 - por isso devem ser estudadas alternativas, como o esquema HMAC (Hash Message Authentication Code).



### Autorização 

No decorrer do projecto foi detectado um erro na implementação do bundle de autorização disponibilizado pelo RavenDB^[ Registo do erro - [https://groups.google.com/d/topic/ravendb/Jvv2xrKaZxY/discussion](https://groups.google.com/d/topic/ravendb/Jvv2xrKaZxY/discussion) o que não permitiu desenvolvimentos nesta área até à data. Mas tendo em conta que o erro já foi corrigido é o próximo passo do projecto.
