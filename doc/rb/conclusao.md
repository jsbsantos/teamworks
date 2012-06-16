Conclusão
=

Trabalho Futuro
-

A versão actual da plataforma permite a edição dos seus dados de uma forma muito básica. 
Até à data de entrega da versão final pretende-se implementar funcionalidades que complementem as que já estão disponíveis. 
As funcionalidades a implementar são:

 * Autorização 

	No decorrer do projecto foi detectado um erro na implementação do *bundle* de autorização disponibilizado pelo RavenDB[^ravendberro] o erro já foi corrigido.

 * Planeamento de tarefas

	Adicionar à gestão de tarefas a possibilidade de lhes atibuir prioridades e que dependam umas das outras. Desta forma é molhorado o controlo e organização das tarefas de um projecto.

 * Análise e Monitorização 

	Pretende-se implementar indicadores para análise de informação sobre o projecto, as suas tarefas e utilizadores para que o responsável por um projecto seja capaz de avaliar qual o estado em que este se encontra.

 * *Dashboard*

	Deverá estar disponível aos utilizadores informação sobre quais os projectos e respectivas tarefas que lhe estão atribuídas para um período de tempo (e.g. semana actual).

Além de novas funcionalidades existem aspectos da solução que têm de ser melhorados. 
A cobertura dos testes unitários deve ser alargada tanto na Api, como na aplicação web e em todos os serviços

A forma de autenticação usada na api web é pouco segura por isso devem ser estudadas alternativas, como a utilização do esquema HMAC (Hash Message Authentication Code) ou Oauth.

[^ravendberro]: O *topic* http://goo.gl/FpIoq no Google Groups do RavenDB detalha o erro e a solução.

