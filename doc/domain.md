# Modelo de domínio

As várias entities, value objects, factories, repositories e relações são representadas no modelo de domínio. Este modelo conceptual representa o vocabulário e os conceitos principais utilizados para a resolução do problema. 

Um aspecto importante do modelo de domínio é que os diagramas e linguagem nele utilizados devem ser perceptível a todos os intervenientes no projecto (e.g. arquitectos de software, programadores, cliente).

As entidades de domínio são:

 * O **Utilizador**, representado pelo username, o email e a password. O email é usado para comunicar com o utilizador e juntamente com a password e pode ser usado para aceder ao sistema como alternativa ao username.

 * O **Projecto**,  é o a raíz da infraestrutura e agrega os utilizadores que a ele estão associados.

 * No contexto de um projecto é possível definir **Tarefas** com um nome e uma descrição. As Tarefas podem ter vários utilizadores associados que podem registar o tempo que despenderam na sua realização. A Tarefa tem também uma previsão do tempo que deve ser despendido a realiza-la e a data em que deve ser concluída.

De forma a manter registo de quando e por quem foram criadas, ou alteradas, as entidades é necessário que todas as entidades tenham propriedades que reflictam essa informação.