
# Registration Directory

Monitoring and reporting of commercial activity information of customers and customers.


## Tech Stack

**Server:** C#, Entityframework Core, PostgreSQL, JSON Web Token, Asp.Net Core API, Docker,  RabbitMQ, Quartz, ClosedXML, MailKit


## Authors

- [@ismailabanoz](https://github.com/ismailabanoz)


## API Reference

#### Create Token

```http
  GET /api/auth
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | **Required**. your username |
| `password` | `string` | **Required**. your password |

#### Get All Commercial Activities
```http
  GET /api/commercialactivities
```
#### Add Commercial Activities
```http
  POST /api/commercialactivities
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `customerid` | `int` | **Required**. your customerid |
| `service` | `string` | **Required**. your service |
| `price` | `decimal` | **Required**. your price |
| `date` | `DateTime` | **Required**. date |

#### Update Commercial Activities
```http
  PUT /api/commercialactivities
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Required**. your id |
| `customerid` | `int` | **Required**. your customerid |
| `service` | `string` | **Required**. your service |
| `price` | `decimal` | **Required**. your price |
| `date` | `DateTime` | **Required**. date |

#### Delete Commercial Activities
```http
  DELETE /api/commercialactivities
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Required**. your id |

#### Get Commercial Activity

```http
  GET /api/commercialactivities/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | **Required**. Id of commercial activity to fetch |


#### Get All Customer
```http
  GET /api/customer
```

#### Add Customer
```http
  POST /api/customer
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `name` | `string` | **Required**. your username |
| `surname` | `string` | **Required**. your password |
| `email` | `string` | **Required**. your email |
| `photograph` | `byte[]` | **Required**. your base64 photo |
| `phone` | `string` | **Required**. your phone |
| `city` | `string` | **Required**. your city |


#### Update Customer
```http
  PUT /api/customer
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Required**. your id |
| `name` | `string` | **Required**. your username |
| `surname` | `string` | **Required**. your password |
| `email` | `string` | **Required**. your email |
| `photograph` | `byte[]` | **Required**. your base64 photo |
| `phone` | `string` | **Required**. your phone |
| `city` | `string` | **Required**. your city |




#### Delete Customer
```http
  DELETE /api/customer
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Required**. your id |

```http
  GET /api/customer/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | **Required**. Id of customer to fetch |