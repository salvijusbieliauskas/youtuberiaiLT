import CardList from "../components/CardList/CardList";
import CategoryList from "../components/CategoryList/CategoryList";
import Search from "../components/Search/Search";
import SortSelection from "../components/SortSelection/SortSelection";

function Home() {
  return (
    <section className="flex flex-col items-center ">
      <div className="text-center text-5xl font-bold py-20">
        <h1>Lietuvos</h1>
        <h1 className="flex justify-center items-center ">
          <span>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              x="0px"
              y="0px"
              width="100"
              height="75"
              viewBox="0 0 48 48"
            >
              <path
                fill="#FF3D00"
                d="M43.2,33.9c-0.4,2.1-2.1,3.7-4.2,4c-3.3,0.5-8.8,1.1-15,1.1c-6.1,0-11.6-0.6-15-1.1c-2.1-0.3-3.8-1.9-4.2-4C4.4,31.6,4,28.2,4,24c0-4.2,0.4-7.6,0.8-9.9c0.4-2.1,2.1-3.7,4.2-4C12.3,9.6,17.8,9,24,9c6.2,0,11.6,0.6,15,1.1c2.1,0.3,3.8,1.9,4.2,4c0.4,2.3,0.9,5.7,0.9,9.9C44,28.2,43.6,31.6,43.2,33.9z"
              ></path>
              <path fill="#FFF" d="M20 31L20 17 32 24z"></path>
            </svg>
          </span>
          <span className="mb-2 ml-[-16px] mr-4">Tuberiai</span>
        </h1>
      </div>
      <article className="2xl:max-w-[30%] max-w-full xl:max-w-[40%] lg:max-w-[60%] md:max-w-[75%] my-3">
        <CategoryList />
      </article>
      <div className="2xl:w-[30%] w-full xl:w-[40%] lg:w-[60%] md:w-[75%] my-3">
        <Search />
        <SortSelection />
      </div>
      <article className="2xl:w-[30%] w-full xl:w-[40%] lg:w-[60%] md:w-[75%] my-3">
        <CardList />
      </article>
    </section>
  );
}
export default Home;
