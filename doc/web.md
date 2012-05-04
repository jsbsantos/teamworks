# Web

Um dos principais objectivos do projecto é que a sua disponiblização seja o mais abrangente possível e para isso é disponibilizado uma aplicação web e uma API RESTful.

A **aplicação web** tem como finalidade disponibilizar ao utilizador uma interface para aceder aos dados da infraestrutura através de qualquer user agent. Para a implementação deste componente é usada a framework ASP.NET MVC lecionada no decorrer do curso. Esta framework, como o próprio nome sugere, implementa o padrão model-view-controller(MVC). 

Na implementação da componente visual da aplicação web é usado HTML5 e CSS3 e o aspecto visual é conseguido utilizando os componentes disponibilizados no kit Twitter Bootstrap. As frameworks javascript jQuery e Knockout permitem tornar a interação com o utilizador mais fluída e interativa.

## ReST Api

ReST (Representational State Transfer) é uma forma de obter informação de uma aplicação web. Acenta sobre o protocolo HTTP e os métodos HTTP (get, post, put, delete, etc) são usados para identificar a ação a realizar sobre o url. Cada url expõe um recurso disponibilizado pela aplicação web. 

Na implementação da API há a preocupação de que o url de acesso ao recurso seja o mais perceptivel por parte do utilizador (e.g. http://host/api/projects/1, http://host/api/projects/1/tasks/3). 

A implementação de uma Api ReST permite tornar acessiveis os recursos da infra estrutura de forma a que esta informação possa ser acedida por qualquer utilizador. O utilizador pode assim:
 * Consumir a informação 
 * Integrar dois sistemas diferentes, a infra estrutura implementada neste projecto com uma outra (e.g. uma aplicação web que possibilite a facturação de serviços pode ser usada para gerar facturas conforme as horas registadas)
 * Organizar a informação de uma forma a que consiga melhor interpreta-la.
 * Disponibilizar a informação num dispositivo móvel.

Devido a estas características optou-se por, em paralelo com a aplicação web desenvolver uma API ReST. 
