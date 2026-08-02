package main

import (
	"log"
	"net/http"

	"lacarte/users/api"
	"github.com/go-chi/chi/v5"
)


func main() {
	r := chi.NewRouter()
	r.Get("/users", api.ListUsers)
	r.Post("/users", api.CreateUser)
	r.Get("/users/{id}", api.GetUser)

	log.Println("listening on :8080")
	var run = http.ListenAndServe(":8080", r)

	if run != nil {
		log.Fatal(run)
	}
}
