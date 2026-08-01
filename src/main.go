package main

import (
	"log"
	"net/http"

	"lacarte/users/api"
)

func registerRoutes(mux *http.ServeMux) {
	mux.HandleFunc("GET /users", api.ListUsers)
	mux.HandleFunc("POST /users", api.CreateUser)
	mux.HandleFunc("GET /users/{id}", api.GetUser)
}

func main() {
	mux := http.NewServeMux()

	registerRoutes(mux)

	log.Println("listening on :8080")
	var run = http.ListenAndServe(":8080", mux)

	if run != nil {
		log.Fatal(run)
	}
}
