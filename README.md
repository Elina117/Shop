***Bug Report***

**Название: Не закреплены элементы управления приложения**

Проект: Shop

Компонент приложения: Любое окно

Статус бага: В работе

Автор: Азат Сайфуллин

Вид бага: Визуальный

Критичность бага: S4.Незначительная (Minor).

Приоритет бага: P2.Средний (Medium)

Описание: Не закреплен элемент скрытия/показа пароля в приложении. При переходе в полноэкранный режим, элемент съезжает.

Шаги воспроизведения: 

1.	Запустить приложение
2.	Открыть его в полный экран

Фактический результат:

Баг воспроизвели: Элементы приложения съезжают с назначенного положения .

![image](https://user-images.githubusercontent.com/113188055/234973075-272f17d1-65ef-4f6a-8e66-eb269a4e3967.png)
![image](https://user-images.githubusercontent.com/113188055/234973127-be7c24c4-e54a-405f-9723-15ee8d298831.png)

Ожидаемый результат:

Элементы приложения будут находится посередине.

![image](https://user-images.githubusercontent.com/113188055/234973180-1feaea39-3646-46de-93de-407edc8bdbe6.png)

**Название: Убрать вылезающее окно с ошибкой после входа.**

Проект: Shop

Компонент приложения: Окно для входа в личный кабинет

Статус бага: Закрыт

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S3.Значительный (Major)

Приоритет бага: P2.Средний (Medium)

Описание: Во время входа в аккаунт, в котором не прописана дата рождения  вылезает ошибка.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Пройти регистрацию(можно пропустить, если в существующем аккаунте не указана дата рождения)
3.	Ввести Логин и Пароль
4.	Нажать на кнопку “Войти”

Фактический результат:

Воспроизвели баг: вылезает ошибка

![image](https://user-images.githubusercontent.com/113188055/234973247-fe06e8a7-8e1c-4352-b320-b9f5f40cf424.png)

Ожидаемый результат:

Вход в аккаунт без ошибок.

![image](https://user-images.githubusercontent.com/113188055/234973316-9ffdf9a2-16b1-4f6f-9379-51472a947cf8.png)

**Название: После входа не появляется данные о пользователе.**
Проект: Shop

Компонент приложения: Личный кабинет

Статус бага: Закрыт

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S2.Критический (Critical)

Приоритет бага: P1.Высокий (High)

Описание: После входа в аккаунт(новый, либо незаполненный дополнительно) отсутствуют данные о пользователе, которые он указывал при регистрации. 

Шаги воспроизведения: 
1.	Запустить приложение
2.	Ввести логин и пароль(если нет логина и пароля, то пройти регистрацию)
3.	Нажать на кнопку “Войти”


Фактический результат:

Воспроизвели баг: Данные о пользователе пустые

![image](https://user-images.githubusercontent.com/113188055/234973379-8d422823-2a1a-4f49-8515-af017d2dd07a.png)

Ожидаемый результат:

В личном кабинете должны быть прописаны поля ,которые мы вводили в регистрации

![image](https://user-images.githubusercontent.com/113188055/234973430-d4269804-c607-4f8d-94a7-8d47f7b08072.png)

**Название: Данные личного кабинета не сохраняются после закрытия приложения и открытия его заново.**

Проект: Shop

Компонент приложения: Личный кабинет

Статус бага: Закрыт

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S2.Критический (Critical)

Приоритет бага: P1.Высокий (High)

Описание: После закрытия приложения и открытия его заново. Данные которые мы редактировали в личном кабинете не сохраняются.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Войти в аккаунт
3.	Нажат на кнопку “Редактировать”
4.	Изменить данные личного кабинета
5.	Нажать на кнопку “Сохранить”
6.	Нажать на кнопку “Выйти”
7.	Закрыть приложение
8.	Запустить приложение заново
9.	Войти в тот же аккаунт

Фактический результат:

Воспроизвели баг: Данные которые мы сохраняли в личном кабинете исчезли.

![image](https://user-images.githubusercontent.com/113188055/234973481-f9ae56a6-c1dd-4d2e-94b5-401421fe296e.png)
![image](https://user-images.githubusercontent.com/113188055/234973508-1d2cff65-b8cb-4e43-b09b-b8bfe99189f9.png)

Ожидаемый результат:

Данные которые мы сохраняли ранее должны остаться.

![image](https://user-images.githubusercontent.com/113188055/234973538-bd3cc5f9-ac4c-45c0-baee-2d0ff47c73a1.png)

**Название: Не работает отправка на почту резервного кода.**

Проект: Shop

Компонент приложения: Восстановление пароля

Статус бага: В работе

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S2.Критический (Critical)

Приоритет бага: P1.Высокий (High)

Описание: Не работает функции отправки резервного кода на почту пользователя данного приложения. Выводит ошибку.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Нажать на кнопку “Забыли пароль?”
3.	Ввести логин и емайл пользователя
4.	Нажать на кнопку “Восстановить”

Фактический результат:

Воспроизвели баг: Вышла ошибка.

![image](https://user-images.githubusercontent.com/113188055/234973590-eb487ee8-1c81-4b22-96e0-7754a74f7307.png)

Ожидаемый результат:

На почту должен отправиться резервный код.

![image](https://user-images.githubusercontent.com/113188055/234973638-11b25b9d-e818-4ab3-9235-a7558c58d360.png)

**Название: Добавить сообщение про незаполненные поля.**

Проект: Shop

Компонент приложения: Восстановление пароля

Статус бага: Закрыто

Автор: Азат Сайфуллин

Вид бага: Дефект UX

Критичность бага:  S4.Незначительный (Minor)

Приоритет бага: P3.Низкий (Low)

Описание: Не выводится сообщение в случае незаполненных полей логина и почты. 

Шаги воспроизведения: 
1.	Запустить приложение
2.	Нажать на кнопку “Забыли пароль?”
3.	Нажать на кнопку “Восстановить”

Фактический результат:

Воспроизвели баг: Не выводится messagebox , что поля не заполнены.

Ожидаемый результат:

Вывод уведомления “Это поле необходимо заполнить”.

![image](https://user-images.githubusercontent.com/113188055/234973756-20f9ad86-891c-44b3-9af5-e1df6ffa7049.png)

**Название: Ошибка при неправильном вводе формата изображения.**

Проект: Shop

Компонент приложения: Добавление товара

Статус бага: Открыто

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S2.Критичный (Critical)

Приоритет бага: P1.Высокий (High)

Описание: Выводится ошибка ,при добавлении одежды, если неправильно укажешь формат изображения.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Ввести логин и пароль
3.	Войти в аккаунт
4.	Нажать на кнопку “Предложить”
5.	Ввести название ,характеристики ,описание и неправильно указать формат изображения.


Фактический результат:

Воспроизвели баг: Выводится ошибка и вылетает программа.

![image](https://user-images.githubusercontent.com/113188055/234973818-20b895e9-336e-4048-817a-8ba995869893.png)
![image](https://user-images.githubusercontent.com/113188055/234973842-1f8a80bc-1ea0-4842-b551-04f3e1ef57cd.png)

Ожидаемый результат:

Добавление фотографии в карточку одежды.

**Название: Кнопки не стоят на заданных местах(перекрывают друг друга, полностью не видно)** 

Проект: Shop

Компонент приложения: Лента рекомендации, личный кабинет

Статус бага: Закрыто

Автор: Азат Сайфуллин

Вид бага: Визуальный

Критичность бага:  S5.Тривиальный (Trivial)

Приоритет бага: P3.Низкий (Low)

Описание: Кнопка “Добавить в избранное” стоит правее, чем надо и кнопку “Редактировать” полностью не видно.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Ввести логин и пароль
3.	Войти в аккаунт(Редактировать)
4.	Нажать на кнопку “Лента”

Фактический результат:

Воспроизвели баг: Текст стоит криво, либо полностью не видно.

![image](https://user-images.githubusercontent.com/113188055/234973943-e1d7cd46-5cdf-42db-a9d9-74a3945df571.png)
![image](https://user-images.githubusercontent.com/113188055/234973968-e438d065-f7aa-46d5-a5df-c410a3a72788.png)

Ожидаемый результат:

Кнопки стоят в правильном положении.

![image](https://user-images.githubusercontent.com/113188055/234974001-71171926-bf4e-4c16-aa31-676d696b7cc2.png)

**Название: Не проверяются входные данные поля “Имя” в личном кабинете.**

Проект: Shop

Компонент приложения: Личный кабинет

Статус бага: Открыто

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S2.Критичный (Critical)

Приоритет бага: P1.Высокий (High)

Описание: В личном кабинете можно изменить поле “Имя” на цифры, такого быть не может.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Ввести логин и пароль
3.	Войти в аккаунт
4.	Нажать на кнопку “Редактировать”
5.	Написать в поле “Имя” цифры
6.	Нажать на кнопку “Сохранить”

Фактический результат:

Воспроизвели баг: в поле “Имя” есть цифры.

![image](https://user-images.githubusercontent.com/113188055/234974057-33e0ddda-f68d-4c23-bdb6-a9758e78bdac.png)

Ожидаемый результат:

Должно выйти уведомление, что в имени не должны быть цифры, символы.

**Название: Не проверяются входные данные поля “Дата рождения” в личном кабинете.**

Проект: Shop

Компонент приложения: Личный кабинет

Статус бага: Открыто

Автор: Азат Сайфуллин

Вид бага: Функциональный

Критичность бага:  S2.Критичный (Critical)

Приоритет бага: P1.Высокий (High)

Описание: В личном кабинете можно изменить поле “Дата рождения” на буквы, такого быть не может.

Шаги воспроизведения: 
1.	Запустить приложение
2.	Ввести логин и пароль
3.	Войти в аккаунт
4.	Нажать на кнопку “Редактировать”
5.	Написать в поле “Дата рождения” буквы
6.	Нажать на кнопку “Сохранить”

Фактический результат:

Воспроизвели баг: в поле “Дата рождения” есть буквы.

![image](https://user-images.githubusercontent.com/113188055/234974180-b9cfc069-faf1-419f-bed7-5afcae6e031b.png)

Ожидаемый результат:

Должно выйти уведомление, что в дате рождения ошибка в написании.
